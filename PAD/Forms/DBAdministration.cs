using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PAD
{
    public partial class DBAdministration : Form
    {
        private string _buffer;
        private TreeNode _currentNode;
        private List<string> _output;

        public DBAdministration()
        {
            InitializeComponent();
            _currentNode = new TreeNode();
            _buffer = string.Empty;
            _output = null;
        }

        private void TreeViewPopulate()
        {
            treeViewTables.Nodes.Clear();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                List<string> tableList = GetTables(dbCon);
                dbCon.Close();
                TreeNode root = treeViewTables.Nodes.Add("TABLES");
                for (int i = 0; i < tableList.Count; i++)
                    root.Nodes.Add(tableList[i]);
                treeViewTables.ExpandAll();
            }
        }

        private List<string> GetTables(SQLiteConnection dbCon)
        {
            DataTable schema = dbCon.GetSchema("Tables");
            List<string> tableNames = new List<string>();
            foreach (DataRow row in schema.Rows)
            {
                tableNames.Add(row[2].ToString());
            }
            return tableNames;
        }

        private void UpdateDataGrid(SQLiteConnection dbCon, DataGridView dgv, string tableName)
        {
            DataSet dataSet = new DataSet();
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter($"select * from {tableName}", dbCon);
            dataAdapter.Fill(dataSet);
            dgv.DataSource = dataSet.Tables[0].DefaultView;
        }

        private void toolStripButtonConnect_Click(object sender, EventArgs e)
        {
            //Utility.DBCreation();
            TreeViewPopulate();
        }

        private void toolStripButtonRefreshTables_Click(object sender, EventArgs e)
        {
            TreeViewPopulate();
        }

        private void toolStripButtonLocalData_Click(object sender, EventArgs e)
        {
            LanguageSelect lSel = new LanguageSelect(CacheLoad._languageList.ToList());
            lSel.ShowDialog(this);
            ELanguage selectedLanguage = lSel.SelectedLanguage;
            LoadSelectedLocalizationIntoDB(selectedLanguage);
        }

        private void LoadSelectedLocalizationIntoDB(ELanguage lang)
        {
            string langDir = lang.ToString();
            Cursor.Current = Cursors.WaitCursor;

            /*
            LanguageDescription ld = new LanguageDescription();
            List<LanguageDescription> ldList = new List<LanguageDescription>();
            string[] tempLDList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\LanguageDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempLDList.Length; i++)
            {
                LanguageDescription temp = ld.ParseFile(tempLDList[i]);
                ldList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    string comm = "insert into LANGUAGE_DESC (LANUAGEID, NAME, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, ldList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < ldList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = ldList[i].LanguageId });
                        command.Parameters.Add(new SQLiteParameter() { Value = ldList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = ldList[i].LanguageCode });
                    }
                    command.ExecuteNonQuery();
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            
            ColorDescription cd = new ColorDescription();
            List<ColorDescription> cdList = new List<ColorDescription>();
            string[] tempCDList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\ColorDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempCDList.Length; i++)
            {
                ColorDescription temp = cd.ParseFile(tempCDList[i]);
                cdList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    string comm = "insert into COLOR_DESC (COLORID, NAME, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, cdList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < cdList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = cdList[i].ColorId });
                        command.Parameters.Add(new SQLiteParameter() { Value = cdList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = cdList[i].LanguageCode });
                    }
                    command.ExecuteNonQuery();
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            
            FontListDescription fld = new FontListDescription();
            List<FontListDescription> fldList = new List<FontListDescription>();
            string[] tempFLDList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\FontListDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempFLDList.Length; i++)
            {
                FontListDescription temp = fld.ParseFile(tempFLDList[i]);
                fldList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    string comm = "insert into FONTLIST_DESC (FONTLISTID, NAME, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, fldList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < fldList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = fldList[i].FontListId });
                        command.Parameters.Add(new SQLiteParameter() { Value = fldList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = fldList[i].LanguageCode });
                    }
                    command.ExecuteNonQuery();
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            
            PlanetDescription pd = new PlanetDescription();
            List<PlanetDescription> pdList = new List<PlanetDescription>();
            string[] tempPDList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\PlanetDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempPDList.Length; i++)
            {
                PlanetDescription temp = pd.ParseFile(tempPDList[i]);
                pdList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    string comm = "insert into PLANET_DESC (PLANETID, NAME, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, pdList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < pdList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = pdList[i].PlanetId });
                        command.Parameters.Add(new SQLiteParameter() { Value = pdList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = pdList[i].LanguageCode });
                    }
                    command.ExecuteNonQuery();
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }

            ZodiakDescription zd = new ZodiakDescription();
            List<ZodiakDescription> zdList = new List<ZodiakDescription>();
            string[] tempZDList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\ZodiakDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempZDList.Length; i++)
            {
                ZodiakDescription temp = zd.ParseFile(tempZDList[i]);
                zdList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    string comm = "insert into ZODIAK_DESC (ZODIAKID, NAME, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, zdList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < zdList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = zdList[i].ZodiakId });
                        command.Parameters.Add(new SQLiteParameter() { Value = zdList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = zdList[i].LanguageCode });
                    }
                    command.ExecuteNonQuery();
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }

            SpecialNavamsha sn = new SpecialNavamsha();
            List<SpecialNavamsha> snList = new List<SpecialNavamsha>();
            string[] tempSNList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\SpecNavamshaDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempSNList.Length; i++)
            {
                SpecialNavamsha temp = sn.ParseFile(tempSNList[i]);
                snList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    string comm = "insert into SPECIALNAVAMSHA_DESC (SPECIALNAVAMSHAID, NAME, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, snList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < snList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = snList[i].SpeciaNavamshaId });
                        command.Parameters.Add(new SQLiteParameter() { Value = snList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = snList[i].LanguageCode });
                    }
                    command.ExecuteNonQuery();
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            
            NakshatraDescription nd = new NakshatraDescription();
            List<NakshatraDescription> ndList = new List<NakshatraDescription>();
            string[] tempNDList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\NakshatraDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempNDList.Length; i++)
            {
                NakshatraDescription temp = nd.ParseFile(tempNDList[i]);
                ndList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = string.Empty;
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 9))})";
                    SQLiteCommand command = null;
                    for (int i = 0; i < ndList.Count; i++)
                    {
                        if ((i % 100) == 0)
                        {
                            if (command != null)
                            {
                                command.ExecuteNonQuery();
                            }
                            int count = (ndList.Count - i) > 100 ? 100 : ndList.Count - i;
                            comm = "insert into NAKSHATRA_DESC (NAKSHATRAID, NAME, SHORTNAME, UPRAVITEL, NATURE, DESCRIPTION, GOODFOR, BADFOR, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, count));
                            command = new SQLiteCommand(comm, dbCon);
                        }
                        command.Parameters.Add(new SQLiteParameter() { Value = ndList[i].NakshatraId });
                        command.Parameters.Add(new SQLiteParameter() { Value = ndList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = ndList[i].ShortName });
                        command.Parameters.Add(new SQLiteParameter() { Value = ndList[i].Upravitel });
                        command.Parameters.Add(new SQLiteParameter() { Value = ndList[i].Nature });
                        command.Parameters.Add(new SQLiteParameter() { Value = ndList[i].Description });
                        command.Parameters.Add(new SQLiteParameter() { Value = ndList[i].GoodFor });
                        command.Parameters.Add(new SQLiteParameter() { Value = ndList[i].BadFor });
                        command.Parameters.Add(new SQLiteParameter() { Value = ndList[i].LanguageCode });
                    }
                    if (command != null)
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            
            MasaDescription md = new MasaDescription();
            List<MasaDescription> mdList = new List<MasaDescription>();
            string[] tempMDList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\MasaDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempMDList.Length; i++)
            {
                MasaDescription temp = md.ParseFile(tempMDList[i]);
                mdList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    string comm = "insert into MASA_DESC (MASAID, NAME, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, mdList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < mdList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = mdList[i].MasaId });
                        command.Parameters.Add(new SQLiteParameter() { Value = mdList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = mdList[i].LanguageCode });
                    }
                    command.ExecuteNonQuery();
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            
            TaraBalaDescription tbd = new TaraBalaDescription();
            List<TaraBalaDescription> tbdList = new List<TaraBalaDescription>();
            string[] tempTBDList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\TarabalaDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempTBDList.Length; i++)
            {
                TaraBalaDescription temp = tbd.ParseFile(tempTBDList[i]);
                tbdList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 5))})";
                    string comm = "insert into TARABALA_DESC (TARABALAID, NAME, SHORTNAME, DESCRIPTION, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, tbdList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < tbdList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = tbdList[i].TaraBalaId });
                        command.Parameters.Add(new SQLiteParameter() { Value = tbdList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = tbdList[i].ShortName });
                        command.Parameters.Add(new SQLiteParameter() { Value = tbdList[i].Description });
                        command.Parameters.Add(new SQLiteParameter() { Value = tbdList[i].LanguageCode });
                    }
                    command.ExecuteNonQuery();
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            
            TithiDescription td = new TithiDescription();
            List<TithiDescription> tdList = new List<TithiDescription>();
            string[] tempTDList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\TithiDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempTDList.Length; i++)
            {
                TithiDescription temp = td.ParseFile(tempTDList[i]);
                tdList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = string.Empty;
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 8))})";
                    SQLiteCommand command = null;
                    for (int i = 0; i < tdList.Count; i++)
                    {
                        if ((i % 100) == 0)
                        {
                            if (command != null)
                            {
                                command.ExecuteNonQuery();
                            }
                            int count = (tdList.Count - i) > 100 ? 100 : tdList.Count - i;
                            comm = "insert into TITHI_DESC (TITHIID, NAME, SHORTNAME, UPRAVITEL, TYPE, GOODFOR, BADFOR, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, count));
                            command = new SQLiteCommand(comm, dbCon);
                        }
                        command.Parameters.Add(new SQLiteParameter() { Value = tdList[i].TithiId });
                        command.Parameters.Add(new SQLiteParameter() { Value = tdList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = tdList[i].ShortName });
                        command.Parameters.Add(new SQLiteParameter() { Value = tdList[i].Upravitel });
                        command.Parameters.Add(new SQLiteParameter() { Value = tdList[i].Type });
                        command.Parameters.Add(new SQLiteParameter() { Value = tdList[i].GoodFor });
                        command.Parameters.Add(new SQLiteParameter() { Value = tdList[i].BadFor });
                        command.Parameters.Add(new SQLiteParameter() { Value = tdList[i].LanguageCode });
                    }
                    if (command != null)
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }

            KaranaDescription kd = new KaranaDescription();
            List<KaranaDescription> kdList = new List<KaranaDescription>();
            string[] tempKDList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\KaranaDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempKDList.Length; i++)
            {
                KaranaDescription temp = kd.ParseFile(tempKDList[i]);
                kdList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = string.Empty;
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 6))})";
                    SQLiteCommand command = null;
                    for (int i = 0; i < kdList.Count; i++)
                    {
                        if ((i % 100) == 0)
                        {
                            if (command != null)
                            {
                                command.ExecuteNonQuery();
                            }
                            int count = (kdList.Count - i) > 100 ? 100 : kdList.Count - i;
                            comm = "insert into KARANA_DESC (KARANAID, NAME, UPRAVITEL, GOODFOR, BADFOR, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, count));
                            command = new SQLiteCommand(comm, dbCon);
                        }
                        command.Parameters.Add(new SQLiteParameter() { Value = kdList[i].KaranaId });
                        command.Parameters.Add(new SQLiteParameter() { Value = kdList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = kdList[i].Upravitel });
                        command.Parameters.Add(new SQLiteParameter() { Value = kdList[i].GoodFor });
                        command.Parameters.Add(new SQLiteParameter() { Value = kdList[i].BadFor });
                        command.Parameters.Add(new SQLiteParameter() { Value = kdList[i].LanguageCode });
                    }
                    if (command != null)
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }

            MuhurtaDescription md = new MuhurtaDescription();
            List<MuhurtaDescription> mdList = new List<MuhurtaDescription>();
            string[] tempMDList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\MuhurtaDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempMDList.Length; i++)
            {
                MuhurtaDescription temp = md.ParseFile(tempMDList[i]);
                mdList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 4))})";
                    string comm = "insert into MUHURTA_DESC (MUHURTAID, NAME, SHORTNAME, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, mdList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < mdList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = mdList[i].MuhurtaId });
                        command.Parameters.Add(new SQLiteParameter() { Value = mdList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = mdList[i].ShortName });
                        command.Parameters.Add(new SQLiteParameter() { Value = mdList[i].LanguageCode });
                    }
                    command.ExecuteNonQuery();
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }

            JogaDescription jd = new JogaDescription();
            List<JogaDescription> jdList = new List<JogaDescription>();
            string[] tempJDList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\JogaDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempJDList.Length; i++)
            {
                JogaDescription temp = jd.ParseFile(tempJDList[i]);
                jdList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 5))})";
                    string comm = "insert into JOGA_DESC (JOGAID, NAME, SHORTNAME, DESCRIPTION, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, jdList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < jdList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = jdList[i].JogaId });
                        command.Parameters.Add(new SQLiteParameter() { Value = jdList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = jdList[i].ShortName });
                        command.Parameters.Add(new SQLiteParameter() { Value = jdList[i].Description });
                        command.Parameters.Add(new SQLiteParameter() { Value = jdList[i].LanguageCode });
                    }
                    command.ExecuteNonQuery();
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            
            EclipseDescription od = new EclipseDescription();
            List<EclipseDescription> odList = new List<EclipseDescription>();
            string[] tempODList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\EclipseDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempODList.Length; i++)
            {
                EclipseDescription temp = od.ParseFile(tempODList[i]);
                odList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    string comm = "insert into ECLIPSE_DESC (ECLIPSEID, NAME, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, odList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < odList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = odList[i].EclipseId });
                        command.Parameters.Add(new SQLiteParameter() { Value = odList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = odList[i].LanguageCode });
                    }
                    command.ExecuteNonQuery();
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            
            TranzitDescription trd = new TranzitDescription();
            List<TranzitDescription> trdList = new List<TranzitDescription>();
            string[] tempTRDList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\TranzitDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempTRDList.Length; i++)
            {
                TranzitDescription temp = trd.ParseFile(tempTRDList[i]);
                trdList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = string.Empty;
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    SQLiteCommand command = null;
                    for (int i = 0; i < trdList.Count; i++)
                    {
                        if ((i % 100) == 0)
                        {
                            if (command != null)
                            {
                                command.ExecuteNonQuery();
                            }
                            int count = (trdList.Count - i) > 100 ? 100 : trdList.Count - i;
                            comm = "insert into TRANZIT_DESC (TRANZITID, DESCRIPTION, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, count));
                            command = new SQLiteCommand(comm, dbCon);
                        }
                        command.Parameters.Add(new SQLiteParameter() { Value = trdList[i].TranzitId });
                        command.Parameters.Add(new SQLiteParameter() { Value = trdList[i].Description });
                        command.Parameters.Add(new SQLiteParameter() { Value = trdList[i].LanguageCode });
                    }
                    if (command != null)
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            
            Muhurta30Description md = new Muhurta30Description();
            List<Muhurta30Description> mdList = new List<Muhurta30Description>();
            string[] tempMDList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\30muhurtDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempMDList.Length; i++)
            {
                Muhurta30Description temp = md.ParseFile(tempMDList[i]);
                mdList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = string.Empty;
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 5))})";
                    SQLiteCommand command = null;
                    for (int i = 0; i < mdList.Count; i++)
                    {
                        if ((i % 100) == 0)
                        {
                            if (command != null)
                            {
                                command.ExecuteNonQuery();
                            }
                            int count = (mdList.Count - i) > 100 ? 100 : mdList.Count - i;
                            comm = "insert into MUHURTA30_DESC (MUHURTA30ID, SHORTNAME, NAME, DESCRIPTION, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, count));
                            command = new SQLiteCommand(comm, dbCon);
                        }
                        command.Parameters.Add(new SQLiteParameter() { Value = mdList[i].Muhurta30Id });
                        command.Parameters.Add(new SQLiteParameter() { Value = mdList[i].ShortName });
                        command.Parameters.Add(new SQLiteParameter() { Value = mdList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = mdList[i].Description });
                        command.Parameters.Add(new SQLiteParameter() { Value = mdList[i].LanguageCode });
                    }
                    if (command != null)
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            
            Ghati60Description gd = new Ghati60Description();
            List<Ghati60Description> gdList = new List<Ghati60Description>();
            string[] tempGDList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\60ghatiDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempGDList.Length; i++)
            {
                Ghati60Description temp = gd.ParseFile(tempGDList[i]);
                gdList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = string.Empty;
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 5))})";
                    SQLiteCommand command = null;
                    for (int i = 0; i < gdList.Count; i++)
                    {
                        if ((i % 100) == 0)
                        {
                            if (command != null)
                            {
                                command.ExecuteNonQuery();
                            }
                            int count = (gdList.Count - i) > 100 ? 100 : gdList.Count - i;
                            comm = "insert into GHATI60_DESC (GHATI60ID, SHORTNAME, NAME, DESCRIPTION, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, count));
                            command = new SQLiteCommand(comm, dbCon);
                        }
                        command.Parameters.Add(new SQLiteParameter() { Value = gdList[i].Ghati60Id });
                        command.Parameters.Add(new SQLiteParameter() { Value = gdList[i].ShortName });
                        command.Parameters.Add(new SQLiteParameter() { Value = gdList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = gdList[i].Description });
                        command.Parameters.Add(new SQLiteParameter() { Value = gdList[i].LanguageCode });
                    }
                    if (command != null)
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            
            NityaJogaDescription jnd = new NityaJogaDescription();
            List<NityaJogaDescription> jndList = new List<NityaJogaDescription>();
            string[] tempJNDList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\NityaJogaDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempJNDList.Length; i++)
            {
                NityaJogaDescription temp = jnd.ParseFile(tempJNDList[i]);
                jndList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = string.Empty;
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 6))})";
                    SQLiteCommand command = null;
                    for (int i = 0; i < jndList.Count; i++)
                    {
                        if ((i % 100) == 0)
                        {
                            if (command != null)
                            {
                                command.ExecuteNonQuery();
                            }
                            int count = (jndList.Count - i) > 100 ? 100 : jndList.Count - i;
                            comm = "insert into NITYAJOGA_DESC (NITYAJOGAID, NAME, DEITY, MEANING, DESCRIPTION, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, count));
                            command = new SQLiteCommand(comm, dbCon);
                        }
                        command.Parameters.Add(new SQLiteParameter() { Value = jndList[i].NityaJogaId });
                        command.Parameters.Add(new SQLiteParameter() { Value = jndList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = jndList[i].Deity });
                        command.Parameters.Add(new SQLiteParameter() { Value = jndList[i].Meaning });
                        command.Parameters.Add(new SQLiteParameter() { Value = jndList[i].Description });
                        command.Parameters.Add(new SQLiteParameter() { Value = jndList[i].LanguageCode });
                    }
                    if (command != null)
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            
            DVLineNameDescription dvl = new DVLineNameDescription();
            List<DVLineNameDescription> dvlList = new List<DVLineNameDescription>();
            string[] tempDVLList = File.ReadAllLines(@".\Data\Files\" + langDir + @"\DVNamesDesc.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempDVLList.Length; i++)
            {
                DVLineNameDescription temp = dvl.ParseFile(tempDVLList[i]);
                dvlList.Add(temp);
            }
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = string.Empty;
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 4))})";
                    SQLiteCommand command = null;
                    for (int i = 0; i < dvlList.Count; i++)
                    {
                        if ((i % 100) == 0)
                        {
                            if (command != null)
                            {
                                command.ExecuteNonQuery();
                            }
                            int count = (dvlList.Count - i) > 100 ? 100 : dvlList.Count - i;
                            comm = "insert into DVLINENAMES_DESC (DVLINENAMESID, SHORTNAME, NAME, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, count));
                            command = new SQLiteCommand(comm, dbCon);
                        }
                        command.Parameters.Add(new SQLiteParameter() { Value = dvlList[i].DVLineNameId });
                        command.Parameters.Add(new SQLiteParameter() { Value = dvlList[i].ShortName });
                        command.Parameters.Add(new SQLiteParameter() { Value = dvlList[i].Name });
                        command.Parameters.Add(new SQLiteParameter() { Value = dvlList[i].LanguageCode });
                    }
                    if (command != null)
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            */

            Cursor.Current = Cursors.Default;
        }

        private void toolStripButtonLoadDataFromFiles_Click(object sender, EventArgs e)
        {
            int lbWidth = 0;
            bool isLoad = false;

            Cursor.Current = Cursors.WaitCursor;
            CleanTabPage("tabPageGrid");
            ListBox lbLoad = new ListBox();
            lbLoad.BorderStyle = BorderStyle.None;
            lbLoad.AutoSize = true;
            tabControlResults.TabPages["tabPageGrid"].Controls.Add(lbLoad);

            if (!CheckTableContent("DVLINENAMES"))
            {
                if (LoadDVLineNamesIntoDB())
                {
                    isLoad = true;
                    string text = "Названия полосок загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("APPSETTING"))
            {
                if (LoadAppSettingsIntoDB())
                {
                    isLoad = true;
                    string text = "Конфигурации загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("LANGUAGE"))
            {
                if (LoadLanguageIntoDB())
                {
                    isLoad = true;
                    string text = "Языки загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("COLOR"))
            {
                if (LoadColorsIntoDB())
                {
                    isLoad = true;
                    string text = "Цвета загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("SYSTEMFONT"))
            {
                if (LoadSystemFontIntoDB())
                {
                    isLoad = true;
                    string text = "Системные шрифты загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("FONTLIST"))
            {
                if (LoadFontListIntoDB())
                {
                    isLoad = true;
                    string text = "Конфигурации шрифтов загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("PLANET"))
            {
                if (LoadPlanetIntoDB())
                {
                    isLoad = true;
                    string text = "Названия планет загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("ZODIAK"))
            {
                if (LoadZodiakIntoDB())
                {
                    isLoad = true;
                    string text = "Знаки зодиака загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("NAKSHATRA"))
            {
                if (LoadNakshatraIntoDB())
                {
                    isLoad = true;
                    string text = "Накшатры загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("PADA"))
            {
                if (LoadPadaIntoDB())
                {
                    isLoad = true;
                    string text = "Пады загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("MASA"))
            {
                if (LoadMasaIntoDB())
                {
                    isLoad = true;
                    string text = "Маса загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("TARABALA"))
            {
                if (LoadTaraBalaIntoDB())
                {
                    isLoad = true;
                    string text = "Тара Бала загружена успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("TITHI"))
            {
                if (LoadTithiIntoDB())
                {
                    isLoad = true;
                    string text = "Титхи загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("KARANA"))
            {
                if (LoadKaranaIntoDB())
                {
                    isLoad = true;
                    string text = "Караны загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("MUHURTA"))
            {
                if (LoadMuhurtaIntoDB())
                {
                    isLoad = true;
                    string text = "Мухурты загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("JOGA"))
            {
                if (LoadJogaIntoDB())
                {
                    isLoad = true;
                    string text = "Йоги загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("ECLIPSE"))
            {
                if (LoadEclipseIntoDB())
                {
                    isLoad = true;
                    string text = "Затмения загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("TRANZIT"))
            {
                if (LoadTranzityIntoDB())
                {
                    isLoad = true;
                    string text = "Транзиты планет загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("MRITYUBHAGA"))
            {
                if (LoadMrityuBhagaIntoDB())
                {
                    isLoad = true;
                    string text = "Градусы Мритью Бхага загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("MUHURTA30"))
            {
                if (LoadMuhurta30IntoDB())
                {
                    isLoad = true;
                    string text = "30 мухурт загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }
            if (!CheckTableContent("GHATI60"))
            {
                if (LoadGhati60IntoDB())
                {
                    isLoad = true;
                    string text = "60 гхати загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }

            if (!CheckTableContent("NITYAJOGA"))
            {
                if (LoadNityaJogaIntoDB())
                {
                    isLoad = true;
                    string text = "Нитья йоги загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }

            if (!CheckTableContent("LOCATION"))
            {
                if (LoadLocationsIntoDB())
                {
                    isLoad = true;
                    string text = "Локации загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }

            if (!CheckTableContent("APP_TEXTS"))
            {
                if (LoadAppTextsIntoDB())
                {
                    isLoad = true;
                    string text = "Тексты загружены успешно.";
                    Size tSize = TextRenderer.MeasureText(text, this.Font);
                    if (lbWidth < tSize.Width)
                        lbLoad.Width = tSize.Width + 10;
                    lbLoad.Items.Add(text);
                    lbLoad.Refresh();
                }
            }

            if (isLoad)
            {
                lbLoad.Items.Add("Загрузка завершена!");
                lbLoad.AutoSize = true;
                lbLoad.Refresh();
            }
            else
            {
                CleanTabPage("tabPageGrid");
                Label label = new Label();
                label.AutoSize = true;
                label.Text = "Нет новых данных для загрузки.";
                tabControlResults.TabPages["tabPageGrid"].Controls.Add(label);
            }
            Cursor.Current = Cursors.Default;
        }

        private bool CheckTableContent(string tableName)
        {
            bool hasRows = false;
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand($"select * from {tableName}", dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            hasRows = true;
                        reader.Close();
                    }
                }
                catch(SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return hasRows;
        }

        private bool LoadDVLineNamesIntoDB()
        {
            bool isLoaded = false;
            DVLineNames dvl = new DVLineNames();
            List<DVLineNames> dvlList = new List<DVLineNames>();
            string[] tempDVLList = File.ReadAllLines(@".\Data\Files\DVNames.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempDVLList.Length; i++)
            {
                DVLineNames temp = dvl.ParseFile(tempDVLList[i]);
                dvlList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 1))})";
                    string comm = "insert into DVLINENAMES (CODE) values " + String.Join(", ", Enumerable.Repeat(parameters, dvlList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < dvlList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = dvlList[i].Code });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadAppSettingsIntoDB()
        {
            bool isLoaded = false;
            AppSettingList app = new AppSettingList();
            List<AppSettingList> appList = new List<AppSettingList>();
            string[] tempAppList = File.ReadAllLines(@".\Data\Files\AppSettingList.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempAppList.Length; i++)
            {
                AppSettingList temp = app.ParseFile(tempAppList[i]);
                appList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    string comm = "insert into APPSETTING (GROUPCODE, SETTINGCODE, ACTIVE) values " + String.Join(", ", Enumerable.Repeat(parameters, appList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < appList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = appList[i].GroupCode });
                        command.Parameters.Add(new SQLiteParameter() { Value = appList[i].SettingCode });
                        command.Parameters.Add(new SQLiteParameter() { Value = appList[i].Active });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadLanguageIntoDB()
        {
            bool isLoaded = false;
            Language l = new Language();
            List<Language> lList = new List<Language>();
            string[] tempLList = File.ReadAllLines(@".\Data\Files\Language.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempLList.Length; i++)
            {
                Language temp = l.ParseFile(tempLList[i]);
                lList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 2))})";
                    string comm = "insert into LANGUAGE (LANGUAGECODE, CULTURECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, lList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < lList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = lList[i].LanguageCode });
                        command.Parameters.Add(new SQLiteParameter() { Value = lList[i].CultureCode });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadColorsIntoDB()
        {
            bool isLoaded = false;
            Colors c = new Colors();
            List<Colors> cList = new List<Colors>();
            string[] tempCList = File.ReadAllLines(@".\Data\Files\Color.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempCList.Length; i++)
            {
                Colors temp = c.ParseFile(tempCList[i]);
                cList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 2))})";
                    string comm = "insert into COLOR (CODE, ARGBVALUE) values " + String.Join(", ", Enumerable.Repeat(parameters, cList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < cList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = cList[i].Code });
                        command.Parameters.Add(new SQLiteParameter() { Value = cList[i].ARGBValue });                        
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadSystemFontIntoDB()
        {
            bool isLoaded = false;
            SystemFont sf = new SystemFont();
            List<SystemFont> sfList = new List<SystemFont>();
            string[] tempSFList = File.ReadAllLines(@".\Data\Files\SystemFont.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempSFList.Length; i++)
            {
                SystemFont temp = sf.ParseFile(tempSFList[i]);
                sfList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 2))})";
                    string comm = "insert into SYSTEMFONT (APPMAIN, SYSTEMNAME) values " + String.Join(", ", Enumerable.Repeat(parameters, sfList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < sfList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = sfList[i].AppMain });
                        command.Parameters.Add(new SQLiteParameter() { Value = sfList[i].SystemName });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadFontListIntoDB()
        {
            bool isLoaded = false;
            FontList fl = new FontList();
            List<FontList> flList = new List<FontList>();
            string[] tempFLList = File.ReadAllLines(@".\Data\Files\FontList.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempFLList.Length; i++)
            {
                FontList temp = fl.ParseFile(tempFLList[i]);
                flList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    string comm = "insert into FONTLIST (FONTID, CODE, FONTSTYLEID) values " + String.Join(", ", Enumerable.Repeat(parameters, flList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < flList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = flList[i].FontId });
                        command.Parameters.Add(new SQLiteParameter() { Value = flList[i].Code });
                        command.Parameters.Add(new SQLiteParameter() { Value = flList[i].FontStyleId });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadPlanetIntoDB()
        {
            bool isLoaded = false;
            Planet p = new Planet();
            List<Planet> pList = new List<Planet>();
            string[] tempPList = File.ReadAllLines(@".\Data\Files\Planet.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempPList.Length; i++)
            {
                Planet temp = p.ParseFile(tempPList[i]);
                pList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 1))})";
                    string comm = "insert into PLANET (PLANETCODE) values " + String.Join(", ", Enumerable.Repeat(parameters, pList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < pList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = pList[i].Code });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadZodiakIntoDB()
        {
            bool isLoaded = false;
            Zodiak znak = new Zodiak();
            List<Zodiak> zList = new List<Zodiak>();
            string[] tempZList = File.ReadAllLines(@".\Data\Files\Zodiak.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempZList.Length; i++)
            {
                Zodiak temp = znak.ParseFile(tempZList[i]);
                zList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 1))})";
                    string comm = "insert into ZODIAK (ZODIAKCODE) values " + String.Join(", ", Enumerable.Repeat(parameters, zList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < zList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = zList[i].Code });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadNakshatraIntoDB()
        {
            bool isLoaded = false;
            Nakshatra nak = new Nakshatra();
            List<Nakshatra> nList = new List<Nakshatra>();
            string[] tempNList = File.ReadAllLines(@".\Data\Files\Nakshatra.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempNList.Length; i++)
            {
                Nakshatra temp = nak.ParseFile(tempNList[i]);
                nList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 2))})";
                    string comm = "insert into NAKSHATRA (NAKSHATRACODE, COLORID) values " + String.Join(", ", Enumerable.Repeat(parameters, nList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < nList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = nList[i].Code });
                        command.Parameters.Add(new SQLiteParameter() { Value = nList[i].ColorId });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch(SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadPadaIntoDB()
        {
            bool isLoaded = false;
            Pada p = new Pada();
            List<Pada> pList = new List<Pada>();
            string[] tempPList = File.ReadAllLines(@".\Data\Files\Pada.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempPList.Length; i++)
            {
                Pada temp = p.ParseFile(tempPList[i]);
                pList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 7))})";
                    string comm = "insert into PADA (ZODIAKID, NAKSHATRAID, PADANUMBER, DREKKANA, SPECIALNAVAMSHA, NAVAMSHA, COLORID) values " + String.Join(", ", Enumerable.Repeat(parameters, pList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < pList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = pList[i].ZodiakId });
                        command.Parameters.Add(new SQLiteParameter() { Value = pList[i].NakshatraId });
                        command.Parameters.Add(new SQLiteParameter() { Value = pList[i].PadaNumber });
                        command.Parameters.Add(new SQLiteParameter() { Value = pList[i].Drekkana });
                        command.Parameters.Add(new SQLiteParameter() { Value = pList[i].SpecialNavamsha });
                        command.Parameters.Add(new SQLiteParameter() { Value = pList[i].Navamsha });
                        command.Parameters.Add(new SQLiteParameter() { Value = pList[i].ColorId });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadMasaIntoDB()
        {
            bool isLoaded = false;
            Masa s = new Masa();
            List<Masa> sList = new List<Masa>();
            string[] tempSList = File.ReadAllLines(@".\Data\Files\Masa.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempSList.Length; i++)
            {
                Masa temp = s.ParseFile(tempSList[i]);
                sList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    string comm = "insert into MASA (ZODIAKID, SHUNYANAKSHATRA, SHUNYATITHI) values " + String.Join(", ", Enumerable.Repeat(parameters, sList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < sList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = sList[i].ZodiakId });
                        command.Parameters.Add(new SQLiteParameter() { Value = sList[i].ShunyaNakshatra });
                        command.Parameters.Add(new SQLiteParameter() { Value = sList[i].ShunyaTithi });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadTaraBalaIntoDB()
        {
            bool isLoaded = false;
            TaraBala tb = new TaraBala();
            List<TaraBala> tbList = new List<TaraBala>();
            string[] tempTBList = File.ReadAllLines(@".\Data\Files\Tarabala.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempTBList.Length; i++)
            {
                TaraBala temp = tb.ParseFile(tempTBList[i]);
                tbList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 1))})";
                    string comm = "insert into TARABALA (COLORID) values " + String.Join(", ", Enumerable.Repeat(parameters, tbList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < tbList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = tbList[i].ColorId });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadTithiIntoDB()
        {
            bool isLoaded = false;
            Tithi t = new Tithi();
            List<Tithi> tList = new List<Tithi>();
            string[] tempTList = File.ReadAllLines(@".\Data\Files\Tithi.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempTList.Length; i++)
            {
                Tithi temp = t.ParseFile(tempTList[i]);
                tList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 1))})";
                    string comm = "insert into TITHI (COLORID) values " + String.Join(", ", Enumerable.Repeat(parameters, tList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < tList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = tList[i].ColorId });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadKaranaIntoDB()
        {
            bool isLoaded = false;
            Karana karana = new Karana();
            List<Karana> kList = new List<Karana>();
            string[] tempKList = File.ReadAllLines(@".\Data\Files\Karana.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempKList.Length; i++)
            {
                Karana temp = karana.ParseFile(tempKList[i]);
                kList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    string comm = "insert into KARANA (TITHIID, POSITION, COLORID) values " + String.Join(", ", Enumerable.Repeat(parameters, kList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < kList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = kList[i].TithiId });
                        command.Parameters.Add(new SQLiteParameter() { Value = kList[i].Position });
                        command.Parameters.Add(new SQLiteParameter() { Value = kList[i].ColorId });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadMuhurtaIntoDB()
        {
            bool isLoaded = false;
            Muhurta m = new Muhurta();
            List<Muhurta> mList = new List<Muhurta>();
            string[] tempMList = File.ReadAllLines(@".\Data\Files\Muhurta.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempMList.Length; i++)
            {
                Muhurta temp = m.ParseFile(tempMList[i]);
                mList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 2))})";
                    string comm = "insert into MUHURTA (COLORID, MUHURTACODE) values " + String.Join(", ", Enumerable.Repeat(parameters, mList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < mList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = mList[i].ColorId });
                        command.Parameters.Add(new SQLiteParameter() { Value = mList[i].Code });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadJogaIntoDB()
        {
            bool isLoaded = false;
            Joga j = new Joga();
            List<Joga> jList = new List<Joga>();
            string[] tempJList = File.ReadAllLines(@".\Data\Files\Joga.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempJList.Length; i++)
            {
                Joga temp = j.ParseFile(tempJList[i]);
                jList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 2))})";
                    string comm = "insert into JOGA (COLORID, JOGACODE) values " + String.Join(", ", Enumerable.Repeat(parameters, jList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < jList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = jList[i].ColorId });
                        command.Parameters.Add(new SQLiteParameter() { Value = jList[i].Code });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadEclipseIntoDB()
        {
            bool isLoaded = false;
            Eclipse z = new Eclipse();
            List<Eclipse> zList = new List<Eclipse>();
            string[] tempZList = File.ReadAllLines(@".\Data\Files\Eclipse.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempZList.Length; i++)
            {
                Eclipse temp = z.ParseFile(tempZList[i]);
                zList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = "insert into ECLIPSE (ECLIPSECODE) values " + String.Join(", ", Enumerable.Repeat("(?)", zList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < zList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = zList[i].Code });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadTranzityIntoDB()
        {
            bool isLoaded = false;
            Tranzit tr = new Tranzit();
            List<Tranzit> trList = new List<Tranzit>();
            string[] tempFList = File.ReadAllLines(@".\Data\Files\Tranzit.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempFList.Length; i++)
            {
                Tranzit temp = tr.ParseFile(tempFList[i]);
                trList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 4))})";
                    string comm = "insert into TRANZIT (PLANETID, DOM, COLORID, VEDHA) values " + String.Join(", ", Enumerable.Repeat(parameters, trList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < trList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = trList[i].PlanetId });
                        command.Parameters.Add(new SQLiteParameter() { Value = trList[i].Dom });
                        command.Parameters.Add(new SQLiteParameter() { Value = trList[i].ColorId });
                        command.Parameters.Add(new SQLiteParameter() { Value = trList[i].Vedha });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadMrityuBhagaIntoDB()
        {
            bool isLoaded = false;
            MrityuBhaga mb = new MrityuBhaga();
            List<MrityuBhaga> mbList = new List<MrityuBhaga>();
            string[] tempFList = File.ReadAllLines(@".\Data\Files\MrityuBhaga.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempFList.Length; i++)
            {
                MrityuBhaga temp = mb.ParseFile(tempFList[i]);
                mbList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    string comm = "insert into MRITYUBHAGA (PLANETID, ZODIAKID, DEGREE) values " + String.Join(", ", Enumerable.Repeat(parameters, mbList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < mbList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = mbList[i].PlanetId });
                        command.Parameters.Add(new SQLiteParameter() { Value = mbList[i].ZodiakId });
                        command.Parameters.Add(new SQLiteParameter() { Value = mbList[i].Degree });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }
        private bool LoadMuhurta30IntoDB()
        {
            bool isLoaded = false;
            Muhurta30 m30 = new Muhurta30();
            List<Muhurta30> mList = new List<Muhurta30>();
            string[] tempMList = File.ReadAllLines(@".\Data\Files\30muhurt.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempMList.Length; i++)
            {
                Muhurta30 temp = m30.ParseFile(tempMList[i]);
                mList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 2))})";
                    string comm = "insert into MUHURTA30 (COLORID, MUHURTA30CODE) values " + String.Join(", ", Enumerable.Repeat(parameters, mList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < mList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = mList[i].ColorId });
                        command.Parameters.Add(new SQLiteParameter() { Value = mList[i].Muhurta30Code });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadGhati60IntoDB()
        {
            bool isLoaded = false;
            Ghati60 g60 = new Ghati60();
            List<Ghati60> gList = new List<Ghati60>();
            string[] tempGList = File.ReadAllLines(@".\Data\Files\60ghati.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempGList.Length; i++)
            {
                Ghati60 temp = g60.ParseFile(tempGList[i]);
                gList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    string comm = "insert into GHATI60 (POSITION, COLORID, GHATI60CODE) values " + String.Join(", ", Enumerable.Repeat(parameters, gList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < gList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = gList[i].Position });
                        command.Parameters.Add(new SQLiteParameter() { Value = gList[i].ColorId });
                        command.Parameters.Add(new SQLiteParameter() { Value = gList[i].Ghati60Code });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadNityaJogaIntoDB()
        {
            bool isLoaded = false;
            NityaJoga jn = new NityaJoga();
            List<NityaJoga> jnList = new List<NityaJoga>();
            string[] tempJNList = File.ReadAllLines(@".\Data\Files\NityaJoga.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempJNList.Length; i++)
            {
                NityaJoga temp = jn.ParseFile(tempJNList[i]);
                jnList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 5))})";
                    string comm = "insert into NITYAJOGA (NJCODE, COLORID, NAKSHATRAID, JOGIPLANETID, AVAJOGIPLANETID) values " + String.Join(", ", Enumerable.Repeat(parameters, jnList.Count));
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    for (int i = 0; i < jnList.Count; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter() { Value = jnList[i].Code });
                        command.Parameters.Add(new SQLiteParameter() { Value = jnList[i].ColorId });
                        command.Parameters.Add(new SQLiteParameter() { Value = jnList[i].NakshatraId });
                        command.Parameters.Add(new SQLiteParameter() { Value = jnList[i].JogiPlanetId });
                        command.Parameters.Add(new SQLiteParameter() { Value = jnList[i].AvaJogiPlanetId });
                    }
                    command.ExecuteNonQuery();
                    isLoaded = true;
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }


        private bool LoadLocationsIntoDB()
        {
            bool isLoaded = false;
            Location l = new Location();
            string[] tempFList = File.ReadAllLines(@".\Data\Files\Location.txt", Encoding.GetEncoding(1251));
            List<Location> lList = l.ParseFile(tempFList);

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 8))})";
                    string comm;
                    SQLiteCommand command = null;

                    for (int i = 0; i < lList.Count; i++)
                    {
                        if ((i % 100) == 0)
                        {
                            if (command != null)
                            {
                                command.ExecuteNonQuery();
                                isLoaded = true;
                            }
                            int count = (lList.Count - i) > 100 ? 100 : lList.Count - i;
                            comm = "insert into LOCATION (LOCALITY, LATITUDE, LONGITUDE, REGION, STATE, COUNTRY, COUNTRYCODE, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, count));
                            command = new SQLiteCommand(comm, dbCon);
                        }

                        command.Parameters.Add(new SQLiteParameter() { Value = lList[i].Locality });
                        command.Parameters.Add(new SQLiteParameter() { Value = lList[i].Latitude });
                        command.Parameters.Add(new SQLiteParameter() { Value = lList[i].Longitude });
                        command.Parameters.Add(new SQLiteParameter() { Value = lList[i].Region });
                        command.Parameters.Add(new SQLiteParameter() { Value = lList[i].State });
                        command.Parameters.Add(new SQLiteParameter() { Value = lList[i].Country });
                        command.Parameters.Add(new SQLiteParameter() { Value = lList[i].CountryCode });
                        command.Parameters.Add(new SQLiteParameter() { Value = lList[i].LanguageCode });
                    }
                    if (command != null)
                    {
                        command.ExecuteNonQuery();
                        isLoaded = true;
                    }
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private bool LoadAppTextsIntoDB()
        {
            bool isLoaded = false;
            AppTexts at = new AppTexts();
            List<AppTexts> atList = new List<AppTexts>();
            string[] tempList = File.ReadAllLines(@".\Data\Files\AppTexts.txt", Encoding.GetEncoding(1251));
            for (int i = 0; i < tempList.Length; i++)
            {
                AppTexts temp = at.ParseFile(tempList[i]);
                atList.Add(temp);
            }

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    string comm;
                    SQLiteCommand command = null;

                    for (int i = 0; i < atList.Count; i++)
                    {
                        if ((i % 100) == 0)
                        {
                            if (command != null)
                            {
                                command.ExecuteNonQuery();
                                isLoaded = true;
                            }
                            int count = (atList.Count - i) > 100 ? 100 : atList.Count - i;
                            comm = "insert into APP_TEXTS (NATIVETEXT, FOREIGNTEXT, LANGUAGECODE) values " + String.Join(", ", Enumerable.Repeat(parameters, count));
                            command = new SQLiteCommand(comm, dbCon);
                        }

                        command.Parameters.Add(new SQLiteParameter() { Value = atList[i].NativeText });
                        command.Parameters.Add(new SQLiteParameter() { Value = atList[i].ForeignText });
                        command.Parameters.Add(new SQLiteParameter() { Value = atList[i].LanguageCode });
                    }
                    if (command != null)
                    {
                        command.ExecuteNonQuery();
                        isLoaded = true;
                    }
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            return isLoaded;
        }

        private void toolStripButtonExecuteFile_Click(object sender, EventArgs e)
        {
            string sql;
            SQLiteCommand command;
            OpenFileDialog fo = new OpenFileDialog();
            fo.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
            fo.ShowDialog();
            if (File.Exists(fo.FileName))
            {
                using (StreamReader reader = new StreamReader(fo.FileName, Encoding.UTF8))
                {
                    sql = File.ReadAllText(fo.FileName);
                    using (SQLiteConnection dbCon = Utility.GetSQLConnection())
                    {
                        dbCon.Open();
                        try
                        {
                            command = new SQLiteCommand(sql, dbCon);
                            command.ExecuteNonQuery();
                        }
                        catch (SQLiteException ex){ AddingExceptionLabel(ex); }
                        dbCon.Close();
                    }
                }
            }
            TreeViewPopulate();
        }

        private void AddingExceptionLabel(SQLiteException ex)
        {
            Label exLabel = new Label();
            exLabel.AutoSize = true;
            exLabel.Text = ex.Message;
            tabControlResults.TabPages["tabPageGrid"].Controls.Add(exLabel);
        }

        private void toolStripButtonExecute_Click(object sender, EventArgs e)
        {
            CleanTabPage("tabPageGrid");

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                SQLiteCommand command = new SQLiteCommand { Connection = dbCon };
                try
                {
                    command.CommandText = richTextBoxInput.SelectedText;
                    DataTable table = new DataTable();
                    table.Load(command.ExecuteReader());

                    DataGridView dgvResult = new DataGridView();
                    dgvResult.ReadOnly = true;
                    dgvResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dgvResult.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    dgvResult.RowHeadersVisible = false;
                    dgvResult.AllowUserToAddRows = false;
                    dgvResult.AutoSize = true;
                    dgvResult.DataSource = table;
                    tabControlResults.TabPages["tabPageGrid"].Controls.Add(dgvResult);
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
        }

        private void richTextBoxInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                toolStripButtonExecute_Click(sender, e);
            }
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control && e.KeyCode == Keys.R)
            {
                e.SuppressKeyPress = true;
            }
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control && e.KeyCode == Keys.E)
            {
                e.SuppressKeyPress = true;
            }
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control && e.KeyCode == Keys.L)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void CleanTabPage(string tpKey)
        {
            foreach (Control c in tabControlResults.TabPages[tpKey].Controls)
            {
                if (c is DataGridView || c is Label || c is ListBox)
                    tabControlResults.TabPages[tpKey].Controls.Remove(c);
            }
        } 

        private void TableShow(string tableName)
        {
            CleanTabPage("tabPageGrid");

            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    DataGridView dgv = new DataGridView();
                    dgv.ReadOnly = true;
                    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    dgv.RowHeadersVisible = false;
                    dgv.AllowUserToAddRows = false;
                    dgv.AutoSize = true;
                    
                    tabControlResults.TabPages["tabPageGrid"].Controls.Add(dgv);
                    UpdateDataGrid(dbCon, dgv, tableName);
                }
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
        }

        private void treeViewTables_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TableShow(treeViewTables.SelectedNode.Text);
        }

        private void treeViewTables_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _currentNode = e.Node;
                _currentNode.ContextMenuStrip = contextMenuStripTree;
                contextMenuStripTree.Show();
            }
        }

        private void copyNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _buffer = _currentNode.Text;
        }

        private void deleteNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to delete table? ", "Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                CleanTabPage("tabPageGrid");
                using (SQLiteConnection dbCon = Utility.GetSQLConnection())
                {
                    dbCon.Open();
                    try
                    {
                        SQLiteCommand command = new SQLiteCommand("drop table " + _currentNode.Text, dbCon);
                        command.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                    dbCon.Close();
                }
                TreeViewPopulate();
            }
        }

        private void richTextBoxInput_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                richTextBoxInput.ContextMenuStrip = contextMenuStripInput;
                contextMenuStripInput.Show();
            }
        }

        private void copyInputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _buffer = richTextBoxInput.SelectedText;
        }

        private void pasteInputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxInput.SelectedText = _buffer;
        }

        private void tabControlResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxTextResults.Items.Clear();
            DataGridView dgv = Utility.GetDataGridFromTabPage(tabControlResults, "tabPageGrid");
            if (dgv != null)
            {
                var result = dgv.Rows.OfType<DataGridViewRow>().Select(
                r => String.Join("|", r.Cells.OfType<DataGridViewCell>().Select(c => c.Value).ToArray())).ToList();

                _output = result.ToList();
                result.ForEach(i => listBoxTextResults.Items.Add(i));
            }
        }

        private void toolStripButtonTextSave_Click(object sender, EventArgs e)
        {
            if (_output != null && _output.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "Text File | *.txt";
                if (save.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter writer = new StreamWriter(save.OpenFile(), Encoding.GetEncoding(1251));
                    _output.ForEach(i => writer.WriteLine(i));
                    writer.Dispose();
                    writer.Close();
                    MessageBox.Show("Данные сохранены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void toolStripButtonExportCalendarsToFile_Click(object sender, EventArgs e)
        {
            YearSelect ySel = new YearSelect();
            ySel.ShowDialog(this);
            int selectedYear = ySel.SelectedYear;
            PrepareUpdateFileForYear(selectedYear);
            MessageBox.Show("Файл апдейта календарей для " + selectedYear + " года создан успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PrepareUpdateFileForYear(int year)
        {
            File.WriteAllText(@".\Update\" + year + ".dt", "01" + Environment.NewLine, Encoding.GetEncoding(1251));
            List<PlanetData> pdList = GetPlanetDataForYear("SUN", year);
            //PrepareProgressBar(pdList.Count);
            for (int i = 0; i < pdList.Count; i++)
            {
                File.AppendAllText(@".\Update\" + year + ".dt", Utility.Encrypt(CreatePlanetDataRowForFile(pdList[i]), true) + Environment.NewLine);
                //toolStripProgressBar.Value = i;
            }

            File.AppendAllText(@".\Update\" + year + ".dt", "02" + Environment.NewLine, Encoding.GetEncoding(1251));
            pdList = GetPlanetDataForYear("MOON", year);
            //PrepareProgressBar(pdList.Count);
            for (int i = 0; i < pdList.Count; i++)
            {
                File.AppendAllText(@".\Update\" + year + ".dt", Utility.Encrypt(CreatePlanetDataRowForFile(pdList[i]), true) + Environment.NewLine);
                //toolStripProgressBar.Value = i;
            }

            File.AppendAllText(@".\Update\" + year + ".dt", "03" + Environment.NewLine, Encoding.GetEncoding(1251));
            pdList = GetPlanetDataForYear("MERCURY", year);
            //PrepareProgressBar(pdList.Count);
            for (int i = 0; i < pdList.Count; i++)
            {
                File.AppendAllText(@".\Update\" + year + ".dt", Utility.Encrypt(CreatePlanetDataRowForFile(pdList[i]), true) + Environment.NewLine);
                //toolStripProgressBar.Value = i;
            }

            File.AppendAllText(@".\Update\" + year + ".dt", "04" + Environment.NewLine, Encoding.GetEncoding(1251));
            pdList = GetPlanetDataForYear("VENUS", year);
            //PrepareProgressBar(pdList.Count);
            for (int i = 0; i < pdList.Count; i++)
            {
                File.AppendAllText(@".\Update\" + year + ".dt", Utility.Encrypt(CreatePlanetDataRowForFile(pdList[i]), true) + Environment.NewLine);
                //toolStripProgressBar.Value = i;
            }

            File.AppendAllText(@".\Update\" + year + ".dt", "05" + Environment.NewLine, Encoding.GetEncoding(1251));
            pdList = GetPlanetDataForYear("MARS", year);
            //PrepareProgressBar(pdList.Count);
            for (int i = 0; i < pdList.Count; i++)
            {
                File.AppendAllText(@".\Update\" + year + ".dt", Utility.Encrypt(CreatePlanetDataRowForFile(pdList[i]), true) + Environment.NewLine);
                //toolStripProgressBar.Value = i;
            }

            File.AppendAllText(@".\Update\" + year + ".dt", "06" + Environment.NewLine, Encoding.GetEncoding(1251));
            pdList = GetPlanetDataForYear("JUPITER", year);
            //PrepareProgressBar(pdList.Count);
            for (int i = 0; i < pdList.Count; i++)
            {
                File.AppendAllText(@".\Update\" + year + ".dt", Utility.Encrypt(CreatePlanetDataRowForFile(pdList[i]), true) + Environment.NewLine);
                //toolStripProgressBar.Value = i;
            }

            File.AppendAllText(@".\Update\" + year + ".dt", "07" + Environment.NewLine, Encoding.GetEncoding(1251));
            pdList = GetPlanetDataForYear("SATURN", year);
            //PrepareProgressBar(pdList.Count);
            for (int i = 0; i < pdList.Count; i++)
            {
                File.AppendAllText(@".\Update\" + year + ".dt", Utility.Encrypt(CreatePlanetDataRowForFile(pdList[i]), true) + Environment.NewLine);
                //toolStripProgressBar.Value = i;
            }

            File.AppendAllText(@".\Update\" + year + ".dt", "08" + Environment.NewLine, Encoding.GetEncoding(1251));
            pdList = GetPlanetDataForYear("RAHUMEAN", year);
            //PrepareProgressBar(pdList.Count);
            for (int i = 0; i < pdList.Count; i++)
            {
                File.AppendAllText(@".\Update\" + year + ".dt", Utility.Encrypt(CreatePlanetDataRowForFile(pdList[i]), true) + Environment.NewLine);
                //toolStripProgressBar.Value = i;
            }

            File.AppendAllText(@".\Update\" + year + ".dt", "09" + Environment.NewLine, Encoding.GetEncoding(1251));
            pdList = GetPlanetDataForYear("KETUMEAN", year);
            //PrepareProgressBar(pdList.Count);
            for (int i = 0; i < pdList.Count; i++)
            {
                File.AppendAllText(@".\Update\" + year + ".dt", Utility.Encrypt(CreatePlanetDataRowForFile(pdList[i]), true) + Environment.NewLine);
                //toolStripProgressBar.Value = i;
            }

            File.AppendAllText(@".\Update\" + year + ".dt", "10" + Environment.NewLine, Encoding.GetEncoding(1251));
            pdList = GetPlanetDataForYear("RAHUTRUE", year);
            //PrepareProgressBar(pdList.Count);
            for (int i = 0; i < pdList.Count; i++)
            {
                File.AppendAllText(@".\Update\" + year + ".dt", Utility.Encrypt(CreatePlanetDataRowForFile(pdList[i]), true) + Environment.NewLine);
                //toolStripProgressBar.Value = i;
            }

            File.AppendAllText(@".\Update\" + year + ".dt", "11" + Environment.NewLine, Encoding.GetEncoding(1251));
            pdList = GetPlanetDataForYear("KETUTRUE", year);
            //PrepareProgressBar(pdList.Count);
            for (int i = 0; i < pdList.Count; i++)
            {
                File.AppendAllText(@".\Update\" + year + ".dt", Utility.Encrypt(CreatePlanetDataRowForFile(pdList[i]), true) + Environment.NewLine);
                //toolStripProgressBar.Value = i;
            }

            File.AppendAllText(@".\Update\" + year + ".dt", "12" + Environment.NewLine, Encoding.GetEncoding(1251));
            List<TithiData> tdList = GetTithiDataForYear(year);
            //PrepareProgressBar(tdList.Count);
            for (int i = 0; i < tdList.Count; i++)
            {
                File.AppendAllText(@".\Update\" + year + ".dt", Utility.Encrypt(CreateTithiDataRowForFile(tdList[i]), true) + Environment.NewLine);
                //toolStripProgressBar.Value = i;
            }

            File.AppendAllText(@".\Update\" + year + ".dt", "13" + Environment.NewLine, Encoding.GetEncoding(1251));
            List<NityaJogaData> jnList = GetNityaJogaDataForYear(year);
            //PrepareProgressBar(jnList.Count);
            for (int i = 0; i < jnList.Count; i++)
            {
                File.AppendAllText(@".\Update\" + year + ".dt", Utility.Encrypt(CreateNityaJogaDataRowForFile(jnList[i]), true) + Environment.NewLine);
                //toolStripProgressBar.Value = i;
            }

            File.AppendAllText(@".\Update\" + year + ".dt", "14" + Environment.NewLine, Encoding.GetEncoding(1251));
            List<EclipseData> odList = GetEclipseDataForYear(year);
            //PrepareProgressBar(odList.Count);
            for (int i = 0; i < odList.Count; i++)
            {
                File.AppendAllText(@".\Update\" + year + ".dt", Utility.Encrypt(CreateEclipseDataRowForFile(odList[i]), true) + Environment.NewLine);
               // toolStripProgressBar.Value = i;
            }

            // if someting else

            File.AppendAllText(@".\Update\" + year + ".dt", "EOF" + Environment.NewLine, Encoding.GetEncoding(1251));
        }

        private List<PlanetData> GetPlanetDataForYear(string planet, int year)
        {
            string tableName = planet;
            double longitude, latitude, speedinlongitude, speedinlatitude;
            List<PlanetData> pdList = new List<PlanetData>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select DATECHANGE, LONGITUDE, LATITUDE, SPEEDINLONGITUDE, SPEEDINLATITUDE, RETRO, ZODIAKID, NAKSHATRAID, PADAID from {tableName} order by DATECHANGE";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (double.TryParse(reader.StringValue(1), NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out longitude) &&
                                    double.TryParse(reader.StringValue(2), NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out latitude) &&
                                    double.TryParse(reader.StringValue(3), NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out speedinlongitude) &&
                                    double.TryParse(reader.StringValue(4), NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out speedinlatitude))
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
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            DateTime startDate = new DateTime(year, 1, 1, 0, 0, 0);
            DateTime endDate = startDate.AddYears(+1);
            List<PlanetData> resList = pdList.Where(i => i.Date > startDate && i.Date < endDate).ToList();
            return resList;
        }

        private List<TithiData> GetTithiDataForYear(int year)
        {
            double msdifference;
            List<TithiData> tdList = new List<TithiData>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select DATECHANGE, MSDIFFERENCE, TITHIID from TITHIDATA order by DATECHANGE";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (double.TryParse(reader.StringValue(1), NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out msdifference))
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
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            DateTime startDate = new DateTime(year, 1, 1, 0, 0, 0);
            DateTime endDate = startDate.AddYears(+1);
            List<TithiData> resList = tdList.Where(i => i.Date > startDate && i.Date < endDate).ToList();
            return resList;
        }

        private List<NityaJogaData> GetNityaJogaDataForYear(int year)
        {
            double longitude;
            List<NityaJogaData> jnList = new List<NityaJogaData>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select DATECHANGE, LONGITUDE, NAKSHATRAID from NYDATA order by DATECHANGE";
                    SQLiteCommand command = new SQLiteCommand(comm, dbCon);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (double.TryParse(reader.StringValue(1), NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out longitude))
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
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            DateTime startDate = new DateTime(year, 1, 1, 0, 0, 0);
            DateTime endDate = startDate.AddYears(+1);
            List<NityaJogaData> resList = jnList.Where(i => i.Date > startDate && i.Date < endDate).ToList();
            return resList;
        }

        private List<EclipseData> GetEclipseDataForYear(int year)
        {
            List<EclipseData> odList = new List<EclipseData>();
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string comm = $"select DATE, ECLIPSEID from ECLIPSEDATA order by DATE";
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
                catch (SQLiteException ex) { AddingExceptionLabel(ex); }
                dbCon.Close();
            }
            DateTime startDate = new DateTime(year, 1, 1, 0, 0, 0);
            DateTime endDate = startDate.AddYears(+1);
            List<EclipseData> resList = odList.Where(i => i.Date > startDate && i.Date < endDate).ToList();
            return resList;
        }

        private string CreatePlanetDataRowForFile(PlanetData pdItem)
        {
            return String.Join("|",
                             pdItem.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                             pdItem.Longitude.ToString("G", CultureInfo.InvariantCulture),
                             pdItem.Latitude.ToString("G", CultureInfo.InvariantCulture),
                             pdItem.SpeedInLongitude.ToString("G", CultureInfo.InvariantCulture),
                             pdItem.SpedInLatitude.ToString("G", CultureInfo.InvariantCulture),
                             pdItem.Retro,
                             pdItem.ZodiakId,
                             pdItem.NakshatraId,
                             pdItem.PadaId);
        }

        private string CreateTithiDataRowForFile(TithiData tdItem)
        {
            return String.Join("|",
                            tdItem.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                            tdItem.MoonSunDifference.ToString("G", CultureInfo.InvariantCulture),
                            tdItem.TithiId);
        }

        private string CreateNityaJogaDataRowForFile(NityaJogaData jnItem)
        {
            return String.Join("|",
                            jnItem.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                            jnItem.Longitude.ToString("G", CultureInfo.InvariantCulture),
                            jnItem.NakshatraId);
        }

        private string CreateEclipseDataRowForFile(EclipseData odItem)
        {
            return String.Join("|",
                            odItem.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                            odItem.EclipseId);
        }

        private void toolStripButtonImportCalendarFromFile_Click(object sender, EventArgs e)
        {
            ImportCalendarFiles();
        }

        private void ImportCalendarFiles()
        {
            string year = string.Empty;
            string[] filesList = CheckForCalendarsUpdate();
            if (filesList != null)
            {
                for (int i = 0; i < filesList.Length; i++)
                {
                    year += filesList[i].Substring(filesList[i].Length - 7, 4) + " ";
                }

                Cursor.Current = Cursors.WaitCursor;
                for (int i = 0; i < filesList.Length; i++)
                {
                    string[] rowsList = File.ReadAllLines(filesList[i]);
                    UploadNewData(rowsList);

                    // this is just for eclipse
                    //string[] rowsList = File.ReadAllLines(filesList[i], Encoding.GetEncoding(1251));
                    //UploadEclipse(rowsList);
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void UploadEclipse(string[] rowsList)
        {
            EclipseData ed = new EclipseData();
            List<EclipseData> edList = ed.ParseFile(rowsList);
            InsertEclipseResultsIntoDB(edList);
        }

        public void UploadNewData(string[] rowsList)
        {
            string listNum = rowsList[0];
            List<string> tempList = new List<string>();
            
            for (int i = 1; i < rowsList.Length; i++)
            {
                if (rowsList[i].Length == 2 && !rowsList[i].Equals("EOF"))
                {
                    if (tempList.Count > 0)
                    {
                        MakeCalendarList(tempList, Convert.ToInt32(listNum));
                        listNum = rowsList[i];
                        tempList.Clear();
                    }
                }
                if (rowsList[i].Length > 2 && !rowsList[i].Equals("EOF"))
                    tempList.Add(rowsList[i]);
            }
            MakeCalendarList(tempList, Convert.ToInt32(listNum));
        }

        private void MakeCalendarList(List<string> tempList, int listNum)
        {
            PlanetData pd = new PlanetData();
            List<PlanetData> pdList = new List<PlanetData>();
            switch (listNum)
            {
                case 1:
                    pdList = pd.ParseUpdateFile(tempList);
                    InsertResultsIntoDB("SUN", pdList);
                    break;

                case 2:
                    pdList = pd.ParseUpdateFile(tempList);
                    InsertResultsIntoDB("MOON", pdList);
                    break;

                case 3:
                    pdList = pd.ParseUpdateFile(tempList);
                    InsertResultsIntoDB("MERCURY", pdList);
                    break;

                case 4:
                    pdList = pd.ParseUpdateFile(tempList);
                    InsertResultsIntoDB("VENUS", pdList);
                    break;

                case 5:
                    pdList = pd.ParseUpdateFile(tempList);
                    InsertResultsIntoDB("MARS", pdList);
                    break;

                case 6:
                    pdList = pd.ParseUpdateFile(tempList);
                    InsertResultsIntoDB("JUPITER", pdList);
                    break;

                case 7:
                    pdList = pd.ParseUpdateFile(tempList);
                    InsertResultsIntoDB("SATURN", pdList);
                    break;

                case 8:
                    pdList = pd.ParseUpdateFile(tempList);
                    InsertResultsIntoDB("RAHUMEAN", pdList);
                    break;

                case 9:
                    pdList = pd.ParseUpdateFile(tempList);
                    InsertResultsIntoDB("KETUMEAN", pdList);
                    break;

                case 10:
                    pdList = pd.ParseUpdateFile(tempList);
                    InsertResultsIntoDB("RAHUTRUE", pdList);
                    break;

                case 11:
                    pdList = pd.ParseUpdateFile(tempList);
                    InsertResultsIntoDB("KETUTRUE", pdList);
                    break;

                case 12:
                    TithiData td = new TithiData();
                    List<TithiData> tdList = td.ParseUpdateFile(tempList);
                    InsertTithiResultsIntoDB(tdList);
                    break;

                case 13:
                    NityaJogaData jn = new NityaJogaData();
                    List<NityaJogaData> jnList = jn.ParseUpdateFile(tempList);
                    InsertNJResultsIntoDB(jnList);
                    break;

                case 14:
                    EclipseData ed = new EclipseData();
                    List<EclipseData> edList = ed.ParseUpdateFile(tempList);
                    InsertEclipseResultsIntoDB(edList);
                    break;

                case 15:
                    MrityuBhagaData mb = new MrityuBhagaData();
                    List<MrityuBhagaData> mbdList = mb.ParseUpdateFile(tempList);
                    InsertMrityuBhagaResultsIntoDB(mbdList);
                    break;

                default:
                    break;
            }
        }

        private void InsertResultsIntoDB(string planet, List<PlanetData> pDataList)
        {
            string tableName = planet;
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 9))})";
                    string comm;
                    SQLiteCommand command = null;

                    for (int i = 0; i < pDataList.Count; i++)
                    {
                        if ((i % 100) == 0)
                        {
                            if (command != null)
                            {
                                command.ExecuteNonQuery();
                            }
                            int count = (pDataList.Count - i) > 100 ? 100 : pDataList.Count - i;
                            comm = $"insert into {tableName} (DATECHANGE, LONGITUDE, LATITUDE, SPEEDINLONGITUDE, SPEEDINLATITUDE, RETRO, ZODIAKID, NAKSHATRAID, PADAID) values " + String.Join(", ", Enumerable.Repeat(parameters, count));
                            command = new SQLiteCommand(comm, dbCon);
                        }

                        command.Parameters.Add(new SQLiteParameter() { Value = pDataList[i].Date.ToString("yyyy-MM-dd HH:mm:ss") });
                        command.Parameters.Add(new SQLiteParameter() { Value = pDataList[i].Longitude });
                        command.Parameters.Add(new SQLiteParameter() { Value = pDataList[i].Latitude });
                        command.Parameters.Add(new SQLiteParameter() { Value = pDataList[i].SpeedInLongitude });
                        command.Parameters.Add(new SQLiteParameter() { Value = pDataList[i].SpedInLatitude });
                        command.Parameters.Add(new SQLiteParameter() { Value = pDataList[i].Retro });
                        command.Parameters.Add(new SQLiteParameter() { Value = pDataList[i].ZodiakId });
                        command.Parameters.Add(new SQLiteParameter() { Value = pDataList[i].NakshatraId });
                        command.Parameters.Add(new SQLiteParameter() { Value = pDataList[i].PadaId });
                    }
                    if (command != null)
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch { }
                dbCon.Close();
            }
        }

        private void InsertTithiResultsIntoDB(List<TithiData> tdList)
        {
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    string comm;
                    SQLiteCommand command = null;

                    for (int i = 0; i < tdList.Count; i++)
                    {
                        if ((i % 100) == 0)
                        {
                            if (command != null)
                            {
                                command.ExecuteNonQuery();
                            }
                            int count = (tdList.Count - i) > 100 ? 100 : tdList.Count - i;
                            comm = $"insert into TITHIDATA (DATECHANGE, MSDIFFERENCE, TITHIID) values " + String.Join(", ", Enumerable.Repeat(parameters, count));
                            command = new SQLiteCommand(comm, dbCon);
                        }

                        command.Parameters.Add(new SQLiteParameter() { Value = tdList[i].Date.ToString("yyyy-MM-dd HH:mm:ss") });
                        command.Parameters.Add(new SQLiteParameter() { Value = tdList[i].MoonSunDifference });
                        command.Parameters.Add(new SQLiteParameter() { Value = tdList[i].TithiId });
                    }
                    if (command != null)
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch { }
                dbCon.Close();
            }
        }

        private void InsertNJResultsIntoDB(List<NityaJogaData> jnList)
        {
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 3))})";
                    string comm;
                    SQLiteCommand command = null;

                    for (int i = 0; i < jnList.Count; i++)
                    {
                        if ((i % 100) == 0)
                        {
                            if (command != null)
                            {
                                command.ExecuteNonQuery();
                            }
                            int count = (jnList.Count - i) > 100 ? 100 : jnList.Count - i;
                            comm = $"insert into NYDATA (DATECHANGE, LONGITUDE, NAKSHATRAID) values " + String.Join(", ", Enumerable.Repeat(parameters, count));
                            command = new SQLiteCommand(comm, dbCon);
                        }

                        command.Parameters.Add(new SQLiteParameter() { Value = jnList[i].Date.ToString("yyyy-MM-dd HH:mm:ss") });
                        command.Parameters.Add(new SQLiteParameter() { Value = jnList[i].Longitude });
                        command.Parameters.Add(new SQLiteParameter() { Value = jnList[i].NakshatraId });
                    }
                    if (command != null)
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch { }
                dbCon.Close();
            }
        }

        private void InsertEclipseResultsIntoDB(List<EclipseData> edList)
        {
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 2))})";
                    string comm;
                    SQLiteCommand command = null;

                    for (int i = 0; i < edList.Count; i++)
                    {
                        if ((i % 100) == 0)
                        {
                            if (command != null)
                            {
                                command.ExecuteNonQuery();
                            }
                            int count = (edList.Count - i) > 100 ? 100 : edList.Count - i;
                            comm = $"insert into ECLIPSEDATA (DATE, ECLIPSEID) values " + String.Join(", ", Enumerable.Repeat(parameters, count));
                            command = new SQLiteCommand(comm, dbCon);
                        }

                        command.Parameters.Add(new SQLiteParameter() { Value = edList[i].Date.ToString("yyyy-MM-dd HH:mm:ss") });
                        command.Parameters.Add(new SQLiteParameter() { Value = edList[i].EclipseId });
                    }
                    if (command != null)
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch { }
                dbCon.Close();
            }
        }

        private void InsertMrityuBhagaResultsIntoDB(List<MrityuBhagaData> mbdList)
        {
            using (SQLiteConnection dbCon = Utility.GetSQLConnection())
            {
                dbCon.Open();
                try
                {
                    string parameters = $"({String.Join(", ", Enumerable.Repeat("?", 8))})";
                    string comm;
                    SQLiteCommand command = null;

                    for (int i = 0; i < mbdList.Count; i++)
                    {
                        if ((i % 100) == 0)
                        {
                            if (command != null)
                            {
                                command.ExecuteNonQuery();
                            }
                            int count = (mbdList.Count - i) > 100 ? 100 : mbdList.Count - i;
                            comm = $"insert into MRITYUBHAGADATA (PLANETID, ZODIAKID, DEGREE, MRITYUBHAGASETTINGS, LONGITUDEFROM, LONGITUDETO, DATEFROM, DATETO) values " + String.Join(", ", Enumerable.Repeat(parameters, count));
                            command = new SQLiteCommand(comm, dbCon);
                        }
                        command.Parameters.Add(new SQLiteParameter() { Value = mbdList[i].PlanetId });
                        command.Parameters.Add(new SQLiteParameter() { Value = mbdList[i].ZodiakId });
                        command.Parameters.Add(new SQLiteParameter() { Value = mbdList[i].Degree });
                        command.Parameters.Add(new SQLiteParameter() { Value = mbdList[i].MrityuBhagaSetting });
                        command.Parameters.Add(new SQLiteParameter() { Value = mbdList[i].LongitudeFrom });
                        command.Parameters.Add(new SQLiteParameter() { Value = mbdList[i].LongitudeTo });
                        command.Parameters.Add(new SQLiteParameter() { Value = mbdList[i].DateFrom.ToString("yyyy-MM-dd HH:mm:ss") });
                        command.Parameters.Add(new SQLiteParameter() { Value = mbdList[i].DateTo.ToString("yyyy-MM-dd HH:mm:ss") });

                    }
                    if (command != null)
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch { }
                dbCon.Close();
            }
        }

        private string[] CheckForCalendarsUpdate()
        {
            string[] filesList = Directory.GetFiles(@".\Update", "*.dt");
            if (filesList.Length > 0)
                return filesList;
            else
                return null;
        }

        private void DeleteUpdates(string[] filesList)
        {
            for (int i = 0; i < filesList.Length; i++)
                File.Delete(filesList[i]);
        }




    }
}
