using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Diagnostics;
using CsvHelper;
using System.Globalization;
using OpenQA.Selenium.Interactions;
using Keys = OpenQA.Selenium.Keys;
using Domain;
using System.Net;
using AutoUpdaterDotNET;
using System.Configuration;
using System.Collections.Specialized;
using ICSharpCode.SharpZipLib.Zip;
using System.Drawing.Text;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using OpenQA.Selenium.Remote;
using Newtonsoft.Json;
using WIN32 = Microsoft.Win32;

namespace Presentation
{
    public partial class WAButtfrm : Form
    {


        public WA wa = new WA();

        public static string filenameextracted = string.Empty;
        public string filetype;
        public static StringBuilder strex;
        private string FileName = string.Empty;
        private CancellationTokenSource cancellationToken;
        private CancellationTokenSource pauseToken;
        private CancellationTokenSource eachmessagetoken;
        private CancellationTokenSource severalpausetoken;

        private CancellationTokenSource cancellationToken2;
        private CancellationTokenSource pauseToken2;
        private CancellationTokenSource eachmessagetoken2;
        private CancellationTokenSource severalpausetoken2;

        int pausetiming = 0;
        int pausetiming2 = 0;

        bool stopbtnclicked;
        bool stopbtnclicked2;



        int sendedmessage;
        int sendedmessage2;

        int notsendedmessage;
        int notsendedmessage2;

        int rowcount;
        int rowcount2;

        int eachmessagetiming = 0;
        int eachmessagetiming2 = 0;





        private static string actualuser = Environment.UserName;




        public string chromedriverversion;

        public string chromedriverdwlink;

        public static string chromewadefaultuserdata = "https://raw.githubusercontent.com/wabutt/itsmevsauce/master/Chrome%20WA%20Profile.zip";

        public static string chromesmsdefaultuserdata = "https://raw.githubusercontent.com/wabutt/itsmevsauce/master/Chrome%20SMS%20Profile.zip";


        public WAButtfrm()
        {
            AutoUpdater.InstalledVersion = Version.Parse("1.0.0.14");
            UserModel user = new UserModel();





            if (!CheckForInternetConnection())
            {

                DialogResult d;
                d = MessageBox.Show("No cuenta con acceso a internet, le recomendamos intentar mas tarde.", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (d == DialogResult.OK)
                {
                    this.Load += (sender, e) => { this.Close(); };
                    return;
                }


            }

            if (!user.CheckHWID(user.GetMachineGuid()))
            {

                DialogResult d;
                d = MessageBox.Show("Contact to Creator :) trevorcalfan2@gmail.com", "<3", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                if (d == DialogResult.OK)
                {
                    Clipboard.SetText(user.GetMachineGuid());

                    this.Load += (sender, e) => { this.Close(); };
                    return;
                }


            }

            //AddUpdateAppSettings("cv", "true");


            if (!ChromeDriverState())
            {
                this.Load += (sender, e) => { this.Close(); }; return;
            }


            CheckUserProfileExist();

            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;




            updatestart();

            ExecuteStart();






        }




        private void FetchChromeDriverVersion()
        {

            try
            {/*
                WIN32.RegistryKey key = WIN32.Registry.CurrentUser.OpenSubKey(@"Software\Google\Chrome\BLBeacon");
                
                if (key != null)
                {
                    Console.WriteLine(key.GetValue("version"));
                    chromedriverversion = Convert.ToString( key.GetValue("version"));

                    key.Close();
                }*/

                var webRequest = WebRequest.Create(@"https://chromedriver.storage.googleapis.com/LATEST_RELEASE");

                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new StreamReader(content))
                {
                    var strContent = reader.ReadToEnd();
                    chromedriverversion = strContent;
                    Console.WriteLine(Convert.ToString("bitches are bad" + strContent));

                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Observación", MessageBoxButtons.OK, MessageBoxIcon.Hand);

            }



        }
        public bool ChromeDriverState()
        {



            if (Environment.Is64BitOperatingSystem)
            {
                try
                {/*
                    await Task.Run(() =>
                    {
                        
                        
                    });*/

                    FetchChromeDriverVersion();



                    chromedriverdwlink = "https://chromedriver.storage.googleapis.com/" + chromedriverversion + "/chromedriver_win32.zip";
                    Dwchromedriver();



                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Observación", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

            }

            else
            {
                DialogResult msg = MessageBox.Show("El SO actual es Arquitectura 32 bits, actualizar a 64 bits para continuar", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                if (msg == DialogResult.OK)
                {
                    return false;
                }

            }


            return true;




        }
        private void KillWebDriver()
        {
            Process[] _proceses = null;
            _proceses = Process.GetProcessesByName("chromedriver");

            if (_proceses.Length != 0)
            {
                foreach (Process proces in _proceses)
                {
                    proces.Kill();
                }
            }
        }
        private void updatestart()
        {

            try
            {

                AutoUpdater.Mandatory = true;
                AutoUpdater.UpdateMode = Mode.Forced;
                AutoUpdater.ShowSkipButton = false;
                AutoUpdater.ShowRemindLaterButton = false;
                AutoUpdater.DownloadPath = Environment.CurrentDirectory;
                AutoUpdater.Start("https://raw.githubusercontent.com/wabutt/itsmevsauce/master/AutoUpdater.xml");

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
        private string ReadSetting(string key)
        {
            string result = "";
            try
            {

                var appSettings = ConfigurationManager.AppSettings;


                result = appSettings[key] ?? "";

                Console.WriteLine(result + "result");

                return result;

            }
            catch (Exception)
            {
                Console.WriteLine("Error reading app settings");

                return result;
            }


        }


        private void Dwchromedriver()
        {

            try
            {

                KillWebDriver();


                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver";
                DirectoryInfo di = Directory.CreateDirectory(path);



                bool file1 = File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriver.exe");
                bool file2 = File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriver.zip");
                bool file3 = File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriverversion.txt");

                bool cdriverfile = File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriverversion.txt");



                if (!cdriverfile)
                {

                    if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriver.exe"))
                    {
                        using (WebClient Client = new WebClient())
                        {
                            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriverversion.txt", chromedriverversion);
                            Client.DownloadFile(chromedriverdwlink, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriver.zip");

                            var zipFileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriver.zip";

                            var targetDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\";


                            FastZip fastZip = new FastZip();
                            string fileFilter = null;

                            // Will always overwrite if target filenames already exist
                            fastZip.ExtractZip(zipFileName, targetDir, fileFilter);


                            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriver.zip");





                        }

                    }
                    else
                    {


                        if (file1)
                        {

                            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriver.exe");
                            Console.WriteLine("FILES 1 DELETED");
                        }
                        if (file2)
                        {

                            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriver.zip");
                            Console.WriteLine("FILES 2 DELETED");

                        }
                        if (file3)
                        {

                            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriverversion.txt");
                            Console.WriteLine("FILES 3 DELETED");

                        }

                        using (WebClient Client = new WebClient())
                        {
                            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriverversion.txt", chromedriverversion);
                            Client.DownloadFile(chromedriverdwlink, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriver.zip");

                            var zipFileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriver.zip";

                            var targetDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\";


                            FastZip fastZip = new FastZip();
                            string fileFilter = null;

                            // Will always overwrite if target filenames already exist
                            fastZip.ExtractZip(zipFileName, targetDir, fileFilter);


                            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriver.zip");





                        }
                    }


                }
                else
                {

                    string tempcdriverV = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriverversion.txt");

                    if (tempcdriverV != chromedriverversion)
                    {

                        if (file1)
                        {

                            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriver.exe");
                            Console.WriteLine("FILES 1 DELETED");
                        }
                        if (file2)
                        {

                            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriver.zip");
                            Console.WriteLine("FILES 2 DELETED");

                        }
                        if (file3)
                        {

                            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriverversion.txt");
                            Console.WriteLine("FILES 3 DELETED");

                        }


                        if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriver.exe"))
                        {
                            using (WebClient Client = new WebClient())
                            {
                                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriverversion.txt", chromedriverversion);

                                Client.DownloadFile(chromedriverdwlink, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriver.zip");



                                var zipFileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriver.zip";

                                var targetDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\";


                                FastZip fastZip = new FastZip();
                                string fileFilter = null;

                                // Will always overwrite if target filenames already exist
                                fastZip.ExtractZip(zipFileName, targetDir, fileFilter);


                                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\webdriver\\chromedriver.zip");





                            }

                        }





                    }





                }




                // AddUpdateAppSettings("cv", "false");




            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }


        }

        private void AddUpdateAppSettings(string key, string value)
        {


            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        private async void OpenSaved()
        {

            Restoremessages();

            if (File.Exists("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\datac.csv"))
            {
                if (new FileInfo("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\datac.csv").Length > 23)
                {
                    DialogResult d;

                    d = MessageBox.Show("Desea cargar los últimos contactos de WhatsApp", "Observación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (d == DialogResult.Yes)
                    {
                        try
                        {
                            await ReadCSV();


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }

                    }
                }
            }






        }
        private async void OpenSaved2()
        {

            Restoremessages2();

            if (File.Exists("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\datac2.csv"))
            {
                if (new FileInfo("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\datac2.csv").Length > 23)
                {
                    DialogResult d;

                    d = MessageBox.Show("Desea cargar los últimos contactos de Google SMS", "Observación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (d == DialogResult.Yes)
                    {
                        try
                        {
                            await ReadCSV2();


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }

                    }
                }
            }






        }

        public void controls(bool state)
        {

            startbtn.Enabled = state;
            uploadbtn.Enabled = state;
            clearfilenamebtn.Enabled = state;
        }
        private void controls2(bool state)
        {

            start2btn.Enabled = state;
        }

        private void WABotfrm_FormClosing(object sender, FormClosingEventArgs e)
        {


            DialogResult result = MessageBox.Show("¿Estás seguro de salir?", "Observación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Storecontaacts();
                Storemessages();

                StoreSettings();


                wa.CloseWDriver();
                wa.CloseWDriver2();



            }
            else
            {
                e.Cancel = true;
            }



        }
        private void WriteJSONToFile(string data, string filename)
        {

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt";

            DirectoryInfo di = Directory.CreateDirectory(path);

            File.WriteAllText(path + "\\" + filename, data);

        }



        private void StoreSettings()
        {

            /*
            AddUpdateAppSettings("waeachmsgpausecant", eachmessagetimingtxt.Text);
            AddUpdateAppSettings("wafullname", Convert.ToString(sendfullnamecb.Checked));
            AddUpdateAppSettings("waevitblock", Convert.ToString(preventblockcb.Checked));
            AddUpdateAppSettings("wasenddt", Convert.ToString(senddatetimecb.Checked));
            AddUpdateAppSettings("wasendmanymsg", Convert.ToString(manymessagescb.Checked));
            AddUpdateAppSettings("waseveralpausecant", severalpausetxt.Text);

            AddUpdateAppSettings("smseachmsgpausecant", eachmessagetiming2txt.Text);
            AddUpdateAppSettings("smsfullname", Convert.ToString(sendfullname2cb.Checked));
            AddUpdateAppSettings("smsevitblock", Convert.ToString(preventblock2cb.Checked));
            AddUpdateAppSettings("smssenddt", Convert.ToString(senddatetime2cb.Checked));
            AddUpdateAppSettings("smssendmanymsg", Convert.ToString(manymessages2cb.Checked));
            AddUpdateAppSettings("smsseveralpausecant", severalpause2txt.Text);

            */
            DataTable dt = new DataTable();



            dt.Columns.Add("waeachmsgpausecant", typeof(string));
            dt.Columns.Add("wafullname", typeof(string));
            dt.Columns.Add("waevitblock", typeof(string));
            dt.Columns.Add("wasenddt", typeof(string));
            dt.Columns.Add("wasendmanymsg", typeof(string));
            dt.Columns.Add("waseveralpausecant", typeof(string));
            dt.Columns.Add("wafileatt", typeof(string));


            dt.Columns.Add("smseachmsgpausecant", typeof(string));
            dt.Columns.Add("smsfullname", typeof(string));
            dt.Columns.Add("smsevitblock", typeof(string));
            dt.Columns.Add("smssenddt", typeof(string));
            dt.Columns.Add("smssendmanymsg", typeof(string));
            dt.Columns.Add("smsseveralpausecant", typeof(string));

            dt.Columns.Add("filetype", typeof(string));
            dt.Columns.Add("sendonlyattach", typeof(string));

            DataRow row = dt.NewRow();

            row["waeachmsgpausecant"] = eachmessagetimingtxt.Text;
            row["wafullname"] = Convert.ToString(sendfullnamecb.Checked);
            row["waevitblock"] = Convert.ToString(preventblockcb.Checked);
            row["wasenddt"] = Convert.ToString(senddatetimecb.Checked);
            row["wasendmanymsg"] = Convert.ToString(manymessagescb.Checked);
            row["waseveralpausecant"] = severalpausetxt.Text;
            row["wafileatt"] = filenametxt.Text;

            row["smseachmsgpausecant"] = eachmessagetiming2txt.Text;
            row["smsfullname"] = Convert.ToString(sendfullname2cb.Checked);
            row["smsevitblock"] = Convert.ToString(preventblock2cb.Checked);
            row["smssenddt"] = Convert.ToString(senddatetime2cb.Checked);
            row["smssendmanymsg"] = Convert.ToString(manymessages2cb.Checked);
            row["smsseveralpausecant"] = severalpause2txt.Text;
            //row["smsseveralpausecant"] = .Text;

            row["filetype"] = filetype;
            row["sendonlyattach"] = Convert.ToString(sendonlyattachcb.Checked);



            dt.Rows.Add(row);


            string JSONDataTable;
            JSONDataTable = JsonConvert.SerializeObject(dt);


            WriteJSONToFile(JSONDataTable, "UserSettings.json");


        }
        private void OpenSettings()
        {


            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tempfilesWAButt\\" + "UserSettings.json";
            Console.WriteLine(path);

            string jsonReadResult;

            if (File.Exists(path))
            {
                jsonReadResult = File.ReadAllText(path);


                DataTable dt = (DataTable)JsonConvert.DeserializeObject(jsonReadResult, typeof(DataTable));



                eachmessagetimingtxt.Text = Convert.ToString(dt.Rows[0][0]);
                sendfullnamecb.Checked = Convert.ToBoolean(dt.Rows[0][1]);
                preventblockcb.Checked = Convert.ToBoolean(dt.Rows[0][2]);
                senddatetimecb.Checked = Convert.ToBoolean(dt.Rows[0][3]);
                manymessagescb.Checked = Convert.ToBoolean(dt.Rows[0][4]);
                severalpausetxt.Text = Convert.ToString(dt.Rows[0][5]);
                filenametxt.Text = Convert.ToString(dt.Rows[0][6]);

                eachmessagetiming2txt.Text = Convert.ToString(dt.Rows[0][7]);
                sendfullname2cb.Checked = Convert.ToBoolean(dt.Rows[0][8]);
                preventblock2cb.Checked = Convert.ToBoolean(dt.Rows[0][9]);
                senddatetime2cb.Checked = Convert.ToBoolean(dt.Rows[0][10]);
                manymessages2cb.Checked = Convert.ToBoolean(dt.Rows[0][11]);
                severalpause2txt.Text = Convert.ToString(dt.Rows[0][12]);

                filetype = Convert.ToString(dt.Rows[0][13]);
                sendonlyattachcb.Checked = Convert.ToBoolean(dt.Rows[0][14]);


            }
            else
            {
                eachmessagetimingtxt.Text = "30";
                sendfullnamecb.Checked = false;
                preventblockcb.Checked = false;
                senddatetimecb.Checked = false;
                manymessagescb.Checked = false;
                severalpausetxt.Text = "300";
                filenametxt.Text = "";
                filetype = "";
                eachmessagetiming2txt.Text = "30";
                sendfullname2cb.Checked = false;
                preventblock2cb.Checked = false;
                senddatetime2cb.Checked = false;
                manymessages2cb.Checked = false;
                severalpause2txt.Text = "300";
                sendonlyattachcb.Checked = false;
            }






            /*
            eachmessagetimingtxt.Text = ReadSetting("waeachmsgpausecant");
            sendfullnamecb.Checked = Convert.ToBoolean(ReadSetting("wafullname"));
            preventblockcb.Checked = Convert.ToBoolean(ReadSetting("waevitblock"));
            senddatetimecb.Checked = Convert.ToBoolean(ReadSetting("wasenddt"));
            manymessagescb.Checked = Convert.ToBoolean(ReadSetting("wasendmanymsg"));
            severalpausetxt.Text = ReadSetting("waseveralpausecant");

            eachmessagetiming2txt.Text = ReadSetting("smseachmsgpausecant");
            sendfullname2cb.Checked = Convert.ToBoolean(ReadSetting("smsfullname"));
            preventblock2cb.Checked = Convert.ToBoolean(ReadSetting("smsevitblock"));
            senddatetime2cb.Checked = Convert.ToBoolean(ReadSetting("smssenddt"));
            manymessages2cb.Checked = Convert.ToBoolean(ReadSetting("smssendmanymsg"));
            severalpause2txt.Text = ReadSetting("smsseveralpausecant");
            */
        }

        private void uploadbtn_Click(object sender, EventArgs e)
        {

            cmsupload.Show(Cursor.Position.X, Cursor.Position.Y);


        }

        private void imagenYVideoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Archivos Personalizados " +
                "(*.tif;*.pjp;*xbm;*jxl;*.svgz;*.jpg;*.jpeg;*.ico;*.tiff;*.gif;*.svg;*.jfif;*.webp;*.png;*.bmp;*.pjpeg;*.avif;*.m4v;*.mp4;*.3gpp;*.mov) | " +
                "*.tif;*.pjp;*xbm;*jxl;*.svgz;*.jpg;*.jpeg;*.ico;*.tiff;*.gif;*.svg;*.jfif;*.webp;*.png;*.bmp;*.pjpeg;*.avif;*.m4v;*.mp4;*.3gpp;*.mov";

            //Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var size = new FileInfo(ofd.FileName).Length;
                string filename = ofd.FileName;


                if (size < 67108864)
                {
                    filenametxt.Text = filename;
                    filetype = "I";
                }
                else
                {
                    filenametxt.Clear();
                    MessageBox.Show("El tamaño del archivo supera el maximo permitido por WhatsApp de 64 MB", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }


            }
        }

        private void audioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();



            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var size = new FileInfo(ofd.FileName).Length;
                string filename = ofd.FileName;


                if (size < 67108864)
                {
                    filenametxt.Text = filename;

                    filetype = "A";
                }
                else
                {
                    filenametxt.Clear();

                    MessageBox.Show("El tamaño del archivo supera el maximo permitido por WhatsApp de 64 MB", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }




        }

        private void documentoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();



            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var size = new FileInfo(ofd.FileName).Length;
                string filename = ofd.FileName;


                if (size < 67108864)
                {
                    filenametxt.Text = filename;

                    filetype = "D";
                }
                else
                {
                    filenametxt.Clear();

                    MessageBox.Show("El tamaño del archivo supera el maximo permitido por WhatsApp de 64 MB", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }




        private void emojibtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://es.piliapp.com/twitter-symbols/");
        }

        private async void connectwabtn_Click(object sender, EventArgs e)
        {

            if (CheckForInternetConnection())
            {

                wa.CloseWDriver();



                try
                {
                    loadmessagelbl.Text = "Estado: Conectando . . . ";



                    await wa.LaunchBrowser();


                    if (wa.driverstate)
                    {
                        loadmessagelbl.Text = "Estado: Navegador Abierto, escanee código QR o empiece a enviar";
                        controls(true);
                        logoutbtn.Enabled = true;


                    }




                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error :" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                MessageBox.Show("No cuenta con acceso a internet, no puedes continuar.", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }





        }

        private void exportDgvToGmail()
        {






            if (contactsdgv.Rows.Count > 1)
            {


                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("No fue posible escribir datos en el disco." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            bool isgroup = false;
                            string value = "";
                            string headercsv = "Primary Phone,First Name";

                            DataGridViewRow dr = new DataGridViewRow();
                            StreamWriter swOut = new StreamWriter(sfd.FileName);


                            swOut.Write(headercsv);
                            swOut.WriteLine();

                            //write DataGridView rows to csv
                            for (int j = 0; j <= contactsdgv.Rows.Count - 2; j++)
                            {

                                if (j > 0)
                                {
                                    swOut.WriteLine();
                                }

                                dr = contactsdgv.Rows[j];

                                for (int i = 0; i <= contactsdgv.Columns.Count - 2; i++)
                                {

                                    if (i > 0)
                                    {
                                        swOut.Write(",");
                                    }
                                    if (i < 1)
                                    {
                                        if (Convert.ToString(dr.Cells[i].Value).Replace(" ", "").Length > 9)
                                        {

                                            if (Convert.ToString(dr.Cells[i].Value).Contains("+") == false && IsDigitsOnly(Convert.ToString(dr.Cells[i].Value)))
                                            {
                                                swOut.Write("+");
                                            }



                                        }



                                    }
                                    if (i < 1)
                                    {
                                        if (IsDigitsOnly(Convert.ToString(dr.Cells[i].Value)) || Convert.ToString(dr.Cells[i].Value).StartsWith("+"))
                                        {
                                            value = Convert.ToString(dr.Cells[i].Value);
                                        }
                                        else
                                        {
                                            value = "";
                                            isgroup = true;
                                        }
                                    }
                                    else
                                    {
                                        if (!isgroup)
                                        {
                                            value = Convert.ToString(dr.Cells[i].Value);
                                        }
                                        else
                                        {
                                            value = "";
                                        }


                                    }





                                    //replace comma's with spaces
                                    value = value.Replace(',', ' ');
                                    //replace embedded newlines with spaces
                                    value = value.Replace(Environment.NewLine, " ");

                                    swOut.Write(value);




                                }



                            }
                            swOut.Close();
                            MessageBox.Show("Datos exportados correctamente!", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay datos a exportar", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }






        }
        private void exportDgvToGmail2()
        {






            if (contacts2dgv.Rows.Count > 1)
            {


                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("No fue posible escribir datos en el disco." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            bool isgroup = false;
                            string value = "";
                            string headercsv = "Primary Phone,First Name";

                            DataGridViewRow dr = new DataGridViewRow();
                            StreamWriter swOut = new StreamWriter(sfd.FileName);


                            swOut.Write(headercsv);
                            swOut.WriteLine();

                            //write DataGridView rows to csv
                            for (int j = 0; j <= contacts2dgv.Rows.Count - 2; j++)
                            {

                                if (j > 0)
                                {
                                    swOut.WriteLine();
                                }

                                dr = contacts2dgv.Rows[j];

                                for (int i = 0; i <= contacts2dgv.Columns.Count - 2; i++)
                                {

                                    if (i > 0)
                                    {
                                        swOut.Write(",");
                                    }
                                    if (i < 1)
                                    {
                                        if (Convert.ToString(dr.Cells[i].Value).Replace(" ", "").Length > 9)
                                        {

                                            if (Convert.ToString(dr.Cells[i].Value).Contains("+") == false && IsDigitsOnly(Convert.ToString(dr.Cells[i].Value)))
                                            {
                                                swOut.Write("+");
                                            }



                                        }



                                    }
                                    if (i < 1)
                                    {
                                        if (IsDigitsOnly(Convert.ToString(dr.Cells[i].Value)) || Convert.ToString(dr.Cells[i].Value).StartsWith("+"))
                                        {
                                            value = Convert.ToString(dr.Cells[i].Value);
                                        }
                                        else
                                        {
                                            value = "";
                                            isgroup = true;
                                        }
                                    }
                                    else
                                    {
                                        if (!isgroup)
                                        {
                                            value = Convert.ToString(dr.Cells[i].Value);
                                        }
                                        else
                                        {
                                            value = "";
                                        }


                                    }





                                    //replace comma's with spaces
                                    value = value.Replace(',', ' ');
                                    //replace embedded newlines with spaces
                                    value = value.Replace(Environment.NewLine, " ");

                                    swOut.Write(value);




                                }



                            }
                            swOut.Close();
                            MessageBox.Show("Datos exportados correctamente!", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay datos a exportar", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }






        }
        private void ImportGmailToDgv()
        {







            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "CSV (*.csv)|*.csv";
            sfd.FileName = sfd.FileName;


            if (sfd.ShowDialog() == DialogResult.OK)
            {

                if (File.Exists(sfd.FileName))
                {


                    try
                    {
                        maintab.SelectedTab = contactlisttab;

                        string keywords = "First Name,";

                        string keywords2 = "Name;";

                        string keywords3 = "Name,";


                        String path = sfd.FileName;
                        List<String> lines = new List<String>();

                        using (StreamReader reader = new StreamReader(path))
                        {
                            String line;

                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line.Contains(","))
                                {
                                    String[] split = line.Split(',');

                                    if (split[1].Contains("34"))
                                    {
                                        split[1] = "100";
                                        line = String.Join(",", split);
                                    }
                                }

                                lines.Add(line);

                            }

                        }



                        var match = lines.FirstOrDefault(stringToCheck => stringToCheck.Contains(keywords));

                        var match2 = lines.FirstOrDefault(stringToCheck => stringToCheck.Contains(keywords2));

                        var match3 = lines.FirstOrDefault(stringToCheck => stringToCheck.Contains(keywords3));




                        if (match != null)
                        {




                            StreamReader sr = new StreamReader(sfd.FileName);
                            StringBuilder sb = new StringBuilder();


                            string s;

                            contactsdgv.Columns.Clear();


                            contactsdgv.Columns.Add("Column", "Numero o Grupo");
                            contactsdgv.Columns.Add("Column", "Nombre");
                            contactsdgv.Columns.Add("Column", "Enviado (S/N)");



                            int indexphone, indexname;


                            s = sr.ReadLine();

                            string[] strs = s.Split(',');


                            indexphone = strs.ToList().IndexOf("Mobile Phone");
                            indexname = strs.ToList().IndexOf("First Name");




                            while (!sr.EndOfStream)
                            {
                                s = sr.ReadLine();

                                string[] str = s.Split(',');


                                //because the first line is header
                                string str1 = str[0].ToString();
                                if (!str1.Equals("First Name"))
                                {

                                    contactsdgv.Rows.Add(str[indexphone].ToString(), str[indexname].ToString());



                                }
                            }
                            sr.Close();

                            DataGridViewColumn column = contactsdgv.Columns[0];
                            column.Width = 200;



                            DataGridViewColumn column1 = contactsdgv.Columns[1];
                            column1.Width = 350;




                            DataGridViewColumn column2 = contactsdgv.Columns[2];
                            column2.Width = 100;
                            column2.ReadOnly = true;

                            MessageBox.Show("Datos importados!", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }


                        else
                        {


                            if (match2 != null)
                            {






                                StreamReader sr = new StreamReader(sfd.FileName);
                                StringBuilder sb = new StringBuilder();


                                string s;

                                contactsdgv.Columns.Clear();


                                contactsdgv.Columns.Add("Column", "Numero o Grupo");
                                contactsdgv.Columns.Add("Column", "Nombre");
                                contactsdgv.Columns.Add("Column", "Enviado (S/N)");

                                int indexphone, indexname;


                                s = sr.ReadLine();

                                string[] strs = s.Split(',');


                                indexphone = strs.ToList().IndexOf("Phone 1 - Value");
                                indexname = strs.ToList().IndexOf("Name");



                                while (!sr.EndOfStream)
                                {
                                    s = sr.ReadLine();

                                    string[] str = s.Split(';');


                                    //because the first line is header
                                    string str1 = str[0].ToString();

                                    if (!str1.Equals("Name"))
                                    {

                                        contactsdgv.Rows.Add(str[indexphone].ToString(), str[indexname].ToString());




                                    }
                                }
                                sr.Close();

                                DataGridViewColumn column = contactsdgv.Columns[0];
                                column.Width = 200;



                                DataGridViewColumn column1 = contactsdgv.Columns[1];
                                column1.Width = 350;




                                DataGridViewColumn column2 = contactsdgv.Columns[2];
                                column2.Width = 100;
                                column2.ReadOnly = true;

                                MessageBox.Show("Datos importados!", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);










                            }
                            else
                            {

                                if (match3 != null)
                                {


                                    StreamReader sr = new StreamReader(sfd.FileName);
                                    StringBuilder sb = new StringBuilder();


                                    string s;




                                    contactsdgv.Columns.Clear();


                                    contactsdgv.Columns.Add("Column", "Numero o Grupo");
                                    contactsdgv.Columns.Add("Column", "Nombre");
                                    contactsdgv.Columns.Add("Column", "Enviado (S/N)");



                                    int indexphone, indexname;


                                    s = sr.ReadLine();

                                    string[] strs = s.Split(',');


                                    indexphone = strs.ToList().IndexOf("Phone 1 - Value");
                                    indexname = strs.ToList().IndexOf("Name");



                                    while (!sr.EndOfStream)
                                    {


                                        s = sr.ReadLine();

                                        string[] str = s.Split(',');


                                        //because the first line is header
                                        string str1 = str[0].ToString();




                                        if (!str1.Equals("Name"))
                                        {






                                            contactsdgv.Rows.Add(str[indexphone].ToString(), str[indexname].ToString());


                                        }

                                    }
                                    sr.Close();

                                    DataGridViewColumn column = contactsdgv.Columns[0];
                                    column.Width = 200;



                                    DataGridViewColumn column1 = contactsdgv.Columns[1];
                                    column1.Width = 350;




                                    DataGridViewColumn column2 = contactsdgv.Columns[2];
                                    column2.Width = 100;
                                    column2.ReadOnly = true;

                                    MessageBox.Show("Datos importados!", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);




                                }
                                else
                                {
                                    Console.WriteLine("entre a leer el csv");
                                    using (var reader = new StreamReader(sfd.FileName))
                                    using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                                    {
                                        csv.Configuration.Delimiter = ",";

                                        using (var dr = new CsvDataReader(csv))
                                        {

                                            contactsdgv.Columns.Clear();

                                            DataTable dtbl = new DataTable();

                                            dtbl.Load(dr);


                                            contactsdgv.Columns.Add("Column", "Numero o Grupo");
                                            contactsdgv.Columns.Add("Column", "Nombre");
                                            contactsdgv.Columns.Add("Column", "Enviado (S/N)");

                                            for (int i = 0; i < dtbl.Rows.Count; i++)
                                            {

                                                contactsdgv.Rows.Add(Convert.ToString(dtbl.Rows[i]["Primary Phone"]), Convert.ToString(dtbl.Rows[i]["First Name"]));


                                            }


                                            DataGridViewColumn column = contactsdgv.Columns[0];
                                            column.Width = 200;



                                            DataGridViewColumn column1 = contactsdgv.Columns[1];
                                            column1.Width = 350;




                                            DataGridViewColumn column2 = contactsdgv.Columns[2];
                                            column2.Width = 100;
                                            column2.ReadOnly = true;

                                            MessageBox.Show("Datos importados!", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                        }
                                    }
                                }




                            }







                        }









                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }



                }


            }

        }
        private void ImportGmailToDgv2()
        {







            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "CSV (*.csv)|*.csv";
            sfd.FileName = sfd.FileName;


            if (sfd.ShowDialog() == DialogResult.OK)
            {

                if (File.Exists(sfd.FileName))
                {


                    try
                    {
                        main2tab.SelectedTab = contactlist2tab;

                        string keywords = "First Name,";

                        string keywords2 = "Name;";

                        string keywords3 = "Name,";


                        String path = sfd.FileName;
                        List<String> lines = new List<String>();

                        using (StreamReader reader = new StreamReader(path))
                        {
                            String line;

                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line.Contains(","))
                                {
                                    String[] split = line.Split(',');

                                    if (split[1].Contains("34"))
                                    {
                                        split[1] = "100";
                                        line = String.Join(",", split);
                                    }
                                }

                                lines.Add(line);

                            }

                        }



                        var match = lines.FirstOrDefault(stringToCheck => stringToCheck.Contains(keywords));

                        var match2 = lines.FirstOrDefault(stringToCheck => stringToCheck.Contains(keywords2));

                        var match3 = lines.FirstOrDefault(stringToCheck => stringToCheck.Contains(keywords3));




                        if (match != null)
                        {




                            StreamReader sr = new StreamReader(sfd.FileName);
                            StringBuilder sb = new StringBuilder();


                            string s;

                            contacts2dgv.Columns.Clear();


                            contacts2dgv.Columns.Add("Column", "Numero o Grupo");
                            contacts2dgv.Columns.Add("Column", "Nombre");
                            contacts2dgv.Columns.Add("Column", "Enviado (S/N)");



                            int indexphone, indexname;


                            s = sr.ReadLine();

                            string[] strs = s.Split(',');


                            indexphone = strs.ToList().IndexOf("Mobile Phone");
                            indexname = strs.ToList().IndexOf("First Name");




                            while (!sr.EndOfStream)
                            {
                                s = sr.ReadLine();

                                string[] str = s.Split(',');


                                //because the first line is header
                                string str1 = str[0].ToString();
                                if (!str1.Equals("First Name"))
                                {

                                    contacts2dgv.Rows.Add(str[indexphone].ToString(), str[indexname].ToString());



                                }
                            }
                            sr.Close();

                            DataGridViewColumn column = contacts2dgv.Columns[0];
                            column.Width = 200;



                            DataGridViewColumn column1 = contacts2dgv.Columns[1];
                            column1.Width = 350;




                            DataGridViewColumn column2 = contacts2dgv.Columns[2];
                            column2.Width = 100;
                            column2.ReadOnly = true;

                            MessageBox.Show("Datos importados!", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }


                        else
                        {


                            if (match2 != null)
                            {






                                StreamReader sr = new StreamReader(sfd.FileName);
                                StringBuilder sb = new StringBuilder();


                                string s;

                                contacts2dgv.Columns.Clear();


                                contacts2dgv.Columns.Add("Column", "Numero o Grupo");
                                contacts2dgv.Columns.Add("Column", "Nombre");
                                contacts2dgv.Columns.Add("Column", "Enviado (S/N)");

                                int indexphone, indexname;


                                s = sr.ReadLine();

                                string[] strs = s.Split(',');


                                indexphone = strs.ToList().IndexOf("Phone 1 - Value");
                                indexname = strs.ToList().IndexOf("Name");



                                while (!sr.EndOfStream)
                                {
                                    s = sr.ReadLine();

                                    string[] str = s.Split(';');


                                    //because the first line is header
                                    string str1 = str[0].ToString();

                                    if (!str1.Equals("Name"))
                                    {

                                        contacts2dgv.Rows.Add(str[indexphone].ToString(), str[indexname].ToString());




                                    }
                                }
                                sr.Close();

                                DataGridViewColumn column = contacts2dgv.Columns[0];
                                column.Width = 200;



                                DataGridViewColumn column1 = contacts2dgv.Columns[1];
                                column1.Width = 350;




                                DataGridViewColumn column2 = contacts2dgv.Columns[2];
                                column2.Width = 100;
                                column2.ReadOnly = true;

                                MessageBox.Show("Datos importados!", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);










                            }
                            else
                            {

                                if (match3 != null)
                                {


                                    StreamReader sr = new StreamReader(sfd.FileName);
                                    StringBuilder sb = new StringBuilder();


                                    string s;




                                    contactsdgv.Columns.Clear();


                                    contacts2dgv.Columns.Add("Column", "Numero o Grupo");
                                    contacts2dgv.Columns.Add("Column", "Nombre");
                                    contacts2dgv.Columns.Add("Column", "Enviado (S/N)");



                                    int indexphone, indexname;


                                    s = sr.ReadLine();

                                    string[] strs = s.Split(',');


                                    indexphone = strs.ToList().IndexOf("Phone 1 - Value");
                                    indexname = strs.ToList().IndexOf("Name");



                                    while (!sr.EndOfStream)
                                    {


                                        s = sr.ReadLine();

                                        string[] str = s.Split(',');


                                        //because the first line is header
                                        string str1 = str[0].ToString();




                                        if (!str1.Equals("Name"))
                                        {






                                            contacts2dgv.Rows.Add(str[indexphone].ToString(), str[indexname].ToString());


                                        }

                                    }
                                    sr.Close();

                                    DataGridViewColumn column = contacts2dgv.Columns[0];
                                    column.Width = 200;



                                    DataGridViewColumn column1 = contacts2dgv.Columns[1];
                                    column1.Width = 350;




                                    DataGridViewColumn column2 = contacts2dgv.Columns[2];
                                    column2.Width = 100;
                                    column2.ReadOnly = true;

                                    MessageBox.Show("Datos importados!", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);




                                }
                                else
                                {
                                    Console.WriteLine("entre a leer el csv");
                                    using (var reader = new StreamReader(sfd.FileName))
                                    using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                                    {
                                        csv.Configuration.Delimiter = ",";

                                        using (var dr = new CsvDataReader(csv))
                                        {

                                            contacts2dgv.Columns.Clear();

                                            DataTable dtbl = new DataTable();

                                            dtbl.Load(dr);


                                            contacts2dgv.Columns.Add("Column", "Numero o Grupo");
                                            contacts2dgv.Columns.Add("Column", "Nombre");
                                            contacts2dgv.Columns.Add("Column", "Enviado (S/N)");

                                            for (int i = 0; i < dtbl.Rows.Count; i++)
                                            {

                                                contacts2dgv.Rows.Add(Convert.ToString(dtbl.Rows[i]["Primary Phone"]), Convert.ToString(dtbl.Rows[i]["First Name"]));


                                            }


                                            DataGridViewColumn column = contacts2dgv.Columns[0];
                                            column.Width = 200;



                                            DataGridViewColumn column1 = contacts2dgv.Columns[1];
                                            column1.Width = 350;




                                            DataGridViewColumn column2 = contacts2dgv.Columns[2];
                                            column2.Width = 100;
                                            column2.ReadOnly = true;

                                            MessageBox.Show("Datos importados!", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                        }
                                    }
                                }




                            }







                        }









                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }



                }


            }

        }
        private async void startbtn_Click(object sender, EventArgs e)
        {
            cancellationToken = new CancellationTokenSource();
            pauseToken = new CancellationTokenSource();
            eachmessagetoken = new CancellationTokenSource();
            severalpausetoken = new CancellationTokenSource();



            await Excecutesendtask();

        }
        public void InputNumbers(object sender, KeyPressEventArgs e)
        {
            // Get the decimal symbol format defined in your regional settings.
            char decimalSeparator = Convert.ToChar(CultureInfo.CurrentCulture.
                        NumberFormat.NumberDecimalSeparator);
            // Check if pressed key is not a control key, digit key and decimal separator key.
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)
                      )
            {
                // Convert the sender to TextBox.
                TextBox toolTippedControl = sender as TextBox;
                // Create ToolTip parameters.
                string toolTipText = "La casilla solo puede contener los siguientes caracteres:"
                        + "\n\t- Numeros: 0123456789";
                int toolTipPosX = toolTippedControl.Width;
                int toolTipPosY = 0;
                int toolTipDuration = 4000;
                // Create a ToolTip object.
                ToolTip toolTip = new ToolTip
                {
                    // Set ToolTip icon.
                    ToolTipIcon = ToolTipIcon.Warning
                };
                // Pass the created ToolTip parameters and show it.
                toolTip.Show(toolTipText, toolTippedControl, toolTipPosX, toolTipPosY, toolTipDuration);
                // Set Handled method to true to cancel the button press.
                e.Handled = true;
            }
            // Decimal separator symbol must be only one, so:
            // Check if the decimal separator key is pressed.
            // And check if the TextBox already have entered symbol for decimal separator.
            if ((e.KeyChar == decimalSeparator) &&
                    ((sender as TextBox).Text.IndexOf(decimalSeparator) > -1))
            {
                // Set Handled method to true to cancel the button press.
                e.Handled = true;
            }
        }
        private void stopbtn_Click(object sender, EventArgs e)
        {


            if (pausetiming > 0)
            {
                DialogResult a;
                a = MessageBox.Show("Los envios están pausados ¿Desea detener la cola de envios? ", "Observación", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (a == DialogResult.Yes)
                {

                    if (cancellationToken != null)
                    {


                        MessageBox.Show("Los envios se detendrán en breve ", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        pauseToken.Cancel();
                        cancellationToken.Cancel();
                        pausetiming = 0;
                        pausebtn.Enabled = false;
                        stopbtn.Enabled = false;
                        eachmessagetoken.Cancel();
                        severalpausetoken.Cancel();
                        stopbtnclicked = true;
                        uploadbtn.Enabled = true;
                        clearfilenamebtn.Enabled = true;
                        logoutbtn.Enabled = true;
                        connectwabtn.Enabled = true;
                        pausebtn.Text = "Pausar";




                        if (!CheckForInternetConnection())
                        {
                            MessageBox.Show("No cuenta con acceso a internet, recomendamos esperar hasta tener conexión para continuar.", "Sugerencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }

            }
            else
            {
                DialogResult b;
                b = MessageBox.Show("Desea detener los envíos? ", "Observación", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (b == DialogResult.Yes)
                {

                    if (cancellationToken != null)
                    {
                        MessageBox.Show("Los envios se detendrán en breve ", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cancellationToken.Cancel();
                        pausetiming = 0;
                        pausebtn.Enabled = false;
                        stopbtn.Enabled = false;
                        eachmessagetoken.Cancel();
                        severalpausetoken.Cancel();
                        stopbtnclicked = true;
                        logoutbtn.Enabled = true;
                        connectwabtn.Enabled = true;
                        uploadbtn.Enabled = true;
                        clearfilenamebtn.Enabled = true;

                        pausebtn.Text = "Pausar";



                        if (!CheckForInternetConnection())
                        {
                            MessageBox.Show("No cuenta con acceso a internet, recomendamos esperar hasta tener conexión para continuar.", "Sugerencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }

                }

            }



        }

        private void pausebtn_Click(object sender, EventArgs e)
        {


            if (pausetiming > 0)
            {
                if (pauseToken != null)
                {
                    pausebtn.Text = "Pausar";
                    pauseToken.Cancel();
                    pausebtn.Enabled = true;
                    stopbtn.Enabled = true;
                    startbtn.Enabled = false;
                    logoutbtn.Enabled = false;
                    connectwabtn.Enabled = true;
                    uploadbtn.Enabled = true;
                    clearfilenamebtn.Enabled = true;


                }

                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("No cuenta con acceso a internet, recomendamos esperar hasta tener conexión para continuar.", "Sugerencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }

            else
            {
                cmspause.Show(Cursor.Position.X, Cursor.Position.Y);
            }

        }

        private async Task Excecutesendtask()
        {


            if (CheckForInternetConnection())
            {

                //condicionales y token de cancellation

                ClearEmptyRows();


                string actualnumber = "";
                string actualname = "";




                //variables

                string filename = filenametxt.Text;
                stopbtnclicked = false;
                rowcount = Convert.ToInt32(contactsdgv.RowCount) - 1;
                sendpbr.Value = 0;
                sendpbr.Maximum = rowcount;
                totalmessageslbl.Text = rowcount.ToString();
                int count = 0;

                sendedmessage = 0;
                notsendedmessage = 0;




                notsendedmessagelbl.Text = sendedmessage.ToString();
                sendedmessagelbl.Text = notsendedmessage.ToString();






                if (contactsdgv.Rows.Count < 2) { MessageBox.Show("No existen contactos a los que enviar!", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else
                {






                    if (wa.IsBrowserClosed(WA.driver) == false)
                    {
                        if (wa.IfConnected(By.XPath("//header/div[2]/div[1]/span[1]/div[2]/div[1]/span[1]")) == false)
                        {
                            DialogResult d;
                            d = MessageBox.Show("Debe escanear el codigo QR para empezar a enviar", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);


                        }


                        else
                        {

                            startbtn.Enabled = false;
                            pausebtn.Enabled = true;
                            stopbtn.Enabled = true;
                            pausetiming = 0;
                            logoutbtn.Enabled = false;
                            uploadbtn.Enabled = false;
                            clearfilenamebtn.Enabled = false;
                            connectwabtn.Enabled = false;
                            int countmessage = 0;
                            loadmessagelbl.Text = "Estado: Conectado . . .";
                            bool activatemanymessages = false;
                            contactsdgv.AllowUserToAddRows = false;
                            contactsdgv.AllowUserToDeleteRows = false;
                            if (manymessagescb.Checked == true)
                            {
                                if (m2txt.Text == "" || m3txt.Text == "" || m4txt.Text == "" || m5txt.Text == "")
                                {
                                    manymessagescb.Checked = false;
                                    MessageBox.Show("No llenó todos los espacios de mensajes, no se usará la opción <Enviar varios textos en un solo envío>", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);



                                }
                            }



                            foreach (DataGridViewRow fila in contactsdgv.Rows)
                            {


                                if (fila.IsNewRow) continue;




                                if (CheckForInternetConnection())
                                {

                                    if (preventblockcb.Checked == true)
                                    {
                                        wa.preventblocktiming = 4000;
                                    }
                                    else
                                    {
                                        wa.preventblocktiming = 0;
                                    }
                                    if (eachmessagetimingcb.Checked == true)
                                    {
                                        eachmessagetiming = Convert.ToInt32(eachmessagetimingtxt.Text) * 1000;
                                    }
                                    else
                                    {
                                        eachmessagetiming = 0;
                                    }



                                    if (Convert.ToString(fila.Cells[0].Value) != string.Empty)
                                    { actualnumber = Convert.ToString(fila.Cells[0].Value); Console.WriteLine(actualnumber); }
                                    else { actualnumber = "numero vacio"; Console.WriteLine(actualnumber); }

                                    if (Convert.ToString(fila.Cells[1].Value) != string.Empty)
                                    { actualname = Convert.ToString(fila.Cells[1].Value); Console.WriteLine(actualname); }
                                    else { actualname = "nombre vacio"; Console.WriteLine(actualname); }




                                    //mensajes

                                    string m1 = m1txt.Text.Replace("\n", "<br/>");
                                    string m2 = m2txt.Text.Replace("\n", "<br/>");
                                    string m3 = m3txt.Text.Replace("\n", "<br/>");
                                    string m4 = m4txt.Text.Replace("\n", "<br/>");
                                    string m5 = m5txt.Text.Replace("\n", "<br/>");

                                    var messages = new List<string>
                                    {
                                        m1,
                                        m2,
                                        m3,
                                        m4,
                                        m5
                                    };





                                    string actualmessagetosend = "";


                                    //bucle de mensajes 2, 3, 4 , 5 yremplazo con regards y goodbyes



                                    if (manymessagescb.Checked == true && activatemanymessages == true)
                                    {
                                        countmessage++;




                                        if (countmessage == 5)
                                        {
                                            countmessage = 0;

                                            actualmessagetosend = actualmessagetosend + messages[0];


                                            if (sendfullnamecb.Checked == true)
                                            {

                                                if (actualname == "nombre vacio")
                                                {
                                                    actualmessagetosend = Regex.Replace(actualmessagetosend, "{nombre}", "");
                                                }

                                                else
                                                {
                                                    actualmessagetosend = Regex.Replace(actualmessagetosend, "{nombre}", actualname);
                                                }

                                            }
                                            if (senddatetimecb.Checked)
                                            {
                                                DateTime actualdate = getTimeNow();

                                                actualmessagetosend = Regex.Replace(actualmessagetosend, "{fecha}", Convert.ToString(actualdate.ToString("dddd, dd MMMM yyyy HH:mm")));

                                            }




                                            activatemanymessages = false;



                                        }
                                        else
                                        {


                                            actualmessagetosend = actualmessagetosend + messages[countmessage];

                                            if (sendfullnamecb.Checked == true)
                                            {

                                                if (actualname == "nombre vacio")
                                                {
                                                    actualmessagetosend = Regex.Replace(actualmessagetosend, "{nombre}", "");
                                                }

                                                else
                                                {
                                                    actualmessagetosend = Regex.Replace(actualmessagetosend, "{nombre}", actualname);
                                                }

                                            }
                                            if (senddatetimecb.Checked)
                                            {
                                                DateTime actualdate = getTimeNow();

                                                actualmessagetosend = Regex.Replace(actualmessagetosend, "{fecha}", Convert.ToString(actualdate.ToString("dddd, dd MMMM yyyy HH:mm")));

                                            }


                                        }

                                    }
                                    else
                                    {


                                        actualmessagetosend = actualmessagetosend + messages[0];


                                        if (sendfullnamecb.Checked == true)
                                        {

                                            if (actualname == "nombre vacio")
                                            {

                                                actualmessagetosend = Regex.Replace(actualmessagetosend, "{nombre}", "");

                                            }

                                            else
                                            {
                                                actualmessagetosend = Regex.Replace(actualmessagetosend, "{nombre}", actualname);
                                            }

                                        }
                                        if (senddatetimecb.Checked)
                                        {
                                            DateTime actualdate = getTimeNow();

                                            actualmessagetosend = Regex.Replace(actualmessagetosend, "{fecha}", Convert.ToString(actualdate.ToString("dddd, dd MMMM yyyy HH:mm")));

                                        }


                                    }



                                    try
                                    {



                                        loadmessagelbl.Text = "";
                                        loadmessagelbl.Text = "Estado: Conectado . . .";




                                        if (actualnumber != "numero vacio")
                                        {
                                            WA.driver.Manage().Window.Size = new Size(850,650); 
                                            Console.WriteLine("el numero no esta vacio y paso a busca contacto");


                                            await Task.Run(() =>
                                            {
                                                if (pausetiming != 0)
                                                {
                                                    try
                                                    {
                                                        pausetimingaction(pausetiming, pauseToken.Token);
                                                        pausetiming = 0;
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                        Console.WriteLine(ex.Message);
                                                    }

                                                }

                                                try
                                                {
                                                    //Task.Delay(3000).Wait();
                                                    Actions action = new Actions(WA.driver);

                                                    wa.ClickSearchIcon();

                                                    wa.ContactSearch(actualnumber);


                                                    action.SendKeys(Keys.Space).Build().Perform();


                                                    //Task.Delay(3000).Wait();

                                                    Console.WriteLine("doy click en el contacto");



                                                    wa.ContactClick();
                                                    //action.SendKeys(Keys.Enter).Build().Perform();
                                                    //wa.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                                                    // wa.driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
                                                    Task.Delay(2000).Wait();

                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message.ToString());

                                                }









                                            }, cancellationToken.Token);




                                            if (wa.clickstate)
                                            {

                                                Console.WriteLine("entre a escribir");




                                                if (!string.IsNullOrEmpty(filenametxt.Text))
                                                {


                                                    if (filetype == "I")
                                                    {

                                                        await Task.Run(() =>
                                                        {
                                                            if (pausetiming != 0)
                                                            {
                                                                try
                                                                {
                                                                    pausetimingaction(pausetiming, pauseToken.Token);
                                                                    pausetiming = 0;
                                                                }
                                                                catch (Exception ex)
                                                                {

                                                                    Console.WriteLine(ex.Message);
                                                                }

                                                            }





                                                            try
                                                            {




                                                                Actions action = new Actions(WA.driver);





                                                                if (!CheckAttachMessageStatus())
                                                                {
                                                                    wa.ImageMessage(filename);
                                                                    Task.Delay(1000 + wa.preventblocktiming).Wait();


                                                                    wa.ContactSend(By.XPath(WA.SendIADButton));
                                                                }
                                                                else
                                                                {
                                                                    if (GetImageState(filename))
                                                                    {

                                                                        wa.ImageTextMessage(filename, actualmessagetosend);
                                                                        action.SendKeys(".").Build().Perform();
                                                                        action.SendKeys(Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace).Build().Perform();
                                                                        Task.Delay(1000 + wa.preventblocktiming).Wait();


                                                                        wa.ContactSend(By.XPath(WA.SendIADButton));
                                                                    }

                                                                    else if (GetVideoState(filename))
                                                                    {

                                                                        wa.VideoTextMessage(filename, actualmessagetosend);
                                                                        action.SendKeys(".").Build().Perform();
                                                                        action.SendKeys(Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace).Build().Perform();
                                                                        Task.Delay(1000 + wa.preventblocktiming).Wait();

                                                                        Console.WriteLine("only video attached");

                                                                        wa.ContactSend(By.XPath(WA.SendIADButton));
                                                                    }
                                                                    else
                                                                    {
                                                                        wa.ImageMessage(filename);
                                                                        Task.Delay(1000 + wa.preventblocktiming).Wait();


                                                                        wa.ContactSend(By.XPath(WA.SendIADButton));

                                                                        Task.Delay(1000 + wa.preventblocktiming).Wait();


                                                                        wa.ClickSearchIcon();

                                                                        wa.ContactSearch(actualnumber);

                                                                        action.SendKeys(Keys.Space).Build().Perform();

                                                                        wa.ContactClick();



                                                                        Task.Delay(1000 + wa.preventblocktiming).Wait();


                                                                        wa.ContactMessage(actualmessagetosend);

                                                                        action.SendKeys("A").Build().Perform();
                                                                        action.SendKeys(Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace).Build().Perform();



                                                                        Console.WriteLine("solo Mensaje escrito");


                                                                        wa.ContactActionEnter();

                                                                        Console.WriteLine("presione enter para enviar");


                                                                    }


                                                                }






                                                                fila.Cells[2].Value = "S";


                                                                Task.Delay(1000 + wa.preventblocktiming).Wait();


                                                                Console.WriteLine("Envio imagen o video y texto");

                                                            }
                                                            catch (Exception ex)
                                                            {

                                                                Console.WriteLine(ex.Message) ;
                                                            }





                                                        }, cancellationToken.Token);

                                                    }
                                                    if (filetype == "A")
                                                    {

                                                        await Task.Run(() =>
                                                        {
                                                            if (pausetiming != 0)
                                                            {
                                                                try
                                                                {
                                                                    pausetimingaction(pausetiming, pauseToken.Token);
                                                                    pausetiming = 0;
                                                                }
                                                                catch (Exception ex)
                                                                {

                                                                    Console.WriteLine(ex.Message);
                                                                }

                                                            }

                                                            try
                                                            {



                                                                Actions action = new Actions(WA.driver);


                                                                wa.ContactFileAudio(filename);



                                                                wa.ContactSend(By.XPath(WA.SendIADButton));




                                                                Task.Delay(1000 + wa.preventblocktiming).Wait();



                                                                if (!CheckAttachMessageStatus())
                                                                {
                                                                    wa.ClickSearchIcon();

                                                                    wa.ContactSearch(actualnumber);

                                                                    action.SendKeys(Keys.Space).Build().Perform();

                                                                    wa.ContactClick();



                                                                    Task.Delay(1000 + wa.preventblocktiming).Wait();


                                                                    wa.ContactMessage(actualmessagetosend);

                                                                    action.SendKeys("A").Build().Perform();
                                                                    action.SendKeys(Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace).Build().Perform();



                                                                    Console.WriteLine("solo Mensaje escrito");


                                                                    wa.ContactActionEnter();

                                                                    Console.WriteLine("presione enter para enviar");
                                                                }












                                                                fila.Cells[2].Value = "S";



                                                                Task.Delay(1000 + wa.preventblocktiming).Wait();


                                                                Console.WriteLine("Envio audio y texto");



                                                            }
                                                            catch (Exception)
                                                            {


                                                            }


                                                        }, cancellationToken.Token);
                                                    }
                                                    if (filetype == "D")
                                                    {

                                                        await Task.Run(() =>
                                                        {
                                                            if (pausetiming != 0)
                                                            {
                                                                try
                                                                {
                                                                    pausetimingaction(pausetiming, pauseToken.Token);
                                                                    pausetiming = 0;
                                                                }
                                                                catch (Exception ex)
                                                                {

                                                                    Console.WriteLine(ex.Message);
                                                                }

                                                            }

                                                            try
                                                            {


                                                                Actions action = new Actions(WA.driver);



                                                                wa.ContactFile(filename);

                                                                wa.ContactSend(By.XPath(WA.SendIADButton));


                                                                Task.Delay(1000 + wa.preventblocktiming).Wait();





                                                                if (!CheckAttachMessageStatus())
                                                                {


                                                                    wa.ClickSearchIcon();

                                                                    wa.ContactSearch(actualnumber);

                                                                    action.SendKeys(Keys.Space).Build().Perform();


                                                                    wa.ContactClick();

                                                                    Task.Delay(1000 + wa.preventblocktiming).Wait();



                                                                    wa.ContactMessage(actualmessagetosend);

                                                                    action.SendKeys("A").Build().Perform();
                                                                    action.SendKeys(Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace).Build().Perform();



                                                                    Console.WriteLine("solo Mensaje escrito");





                                                                    Task.Delay(1000 + wa.preventblocktiming).Wait();

                                                                    wa.ContactActionEnter();

                                                                }




                                                                Console.WriteLine("presione enter para enviar");



                                                                fila.Cells[2].Value = "S";



                                                                Task.Delay(1000 + wa.preventblocktiming).Wait();



                                                                Console.WriteLine("Envio file y texto");


                                                            }
                                                            catch (Exception)
                                                            {


                                                            }



                                                        }, cancellationToken.Token);
                                                    }



                                                }

                                                else
                                                {

                                                    await Task.Run(() =>
                                                    {


                                                        if (pausetiming != 0)
                                                        {
                                                            try
                                                            {
                                                                pausetimingaction(pausetiming, pauseToken.Token);
                                                                pausetiming = 0;
                                                            }
                                                            catch (Exception ex)
                                                            {

                                                                Console.WriteLine(ex.Message);
                                                            }

                                                        }


                                                        try
                                                        {

                                                            Actions action = new Actions(WA.driver);



                                                            wa.ContactMessage(actualmessagetosend);


                                                            action.SendKeys("A").Build().Perform();
                                                            action.SendKeys(Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace).Build().Perform();



                                                            Console.WriteLine("solo Mensaje escrito");
                                                            Task.Delay(1000 + wa.preventblocktiming).Wait();

                                                            wa.ContactActionEnter();
                                                            Console.WriteLine("presione enter para enviar");
                                                            fila.Cells[2].Value = "S";






                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex.Message);



                                                        }


                                                    }, cancellationToken.Token);


                                                }

                                                sendedmessage++;
                                                sendedmessagelbl.Text = Convert.ToString(sendedmessage);
                                                Console.WriteLine(sendedmessage);

                                            }
                                            else
                                            {
                                                Actions action = new Actions(WA.driver);
                                                action.SendKeys(Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace).Build().Perform();

                                                fila.Cells[2].Value = "N";
                                                notsendedmessage++;
                                                notsendedmessagelbl.Text = Convert.ToString(notsendedmessage);
                                                Console.WriteLine(notsendedmessage);



                                            }



                                        }
                                        else
                                        {
                                            fila.Cells[2].Value = "N";
                                            notsendedmessage++;
                                            notsendedmessagelbl.Text = Convert.ToString(notsendedmessage);
                                            Console.WriteLine(notsendedmessage);
                                        }


                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message.ToString());


                                    }
                                }

                                else
                                {


                                    stopbtn.Enabled = false;
                                    pausebtn.Enabled = false;
                                    startbtn.Enabled = true;
                                    MessageBox.Show("Se detuvieron los envios de WA debido a que no cuenta con acceso a internet.", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    break;
                                }

                                count++;
                                if (count <= rowcount)
                                {
                                    sendpbr.Value = count;
                                }

                                activatemanymessages = true;


                                if (severalpausetxt.Text != "")
                                {


                                    if (fila.Index == Convert.ToInt32(severalpausetxt.Text) && !severalpausetoken.IsCancellationRequested)
                                    {

                                        Console.WriteLine("<<<<<<<<<<<<<<<<<<<este es la cuenta ctual de la fila  " + fila.Index);
                                        MessageBox.Show("El envio se pausó debido al <# mensajes para Pausar> designado en esta sección.\nRecomendamos esta pausa para no ser bloqueado en WhatsApp.\nLa pausa suele durar 15 minutos y se empezo el <" + getTimeNow() + ">, actualmente se pausa cada " + severalpausetxt.Text + " mensajes", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                        await Task.Run(() =>
                                            {
                                                try
                                                {
                                                    Task.Delay(TimeSpan.FromSeconds(900), severalpausetoken.Token).Wait();

                                                }
                                                catch (Exception ex)
                                                {

                                                    Console.WriteLine(ex.Message);
                                                }


                                            });



                                    }



                                }
                                if (wa.clickstate)
                                {
                                    await Task.Run(() =>
                                    {
                                        try
                                        {
                                            Task.Delay(eachmessagetiming, eachmessagetoken.Token).Wait();


                                        }
                                        catch (Exception ex)
                                        {

                                            Console.WriteLine(ex.Message.ToString());
                                        }




                                    });
                                }



                            }



                            if (stopbtnclicked != true)
                            {
                                MessageBox.Show("Mensajes enviados correctamente! ", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                uploadbtn.Enabled = true;
                                clearfilenamebtn.Enabled = true;
                                stopbtn.Enabled = false;
                                pausebtn.Enabled = false;
                                startbtn.Enabled = true;
                                logoutbtn.Enabled = true;
                                connectwabtn.Enabled = true;

                            }
                            else
                            {
                                startbtn.Enabled = true;
                            }

                            notsendedmessagelbl.Text = Convert.ToString(rowcount - sendedmessage);

                            contactsdgv.AllowUserToAddRows = true;
                            contactsdgv.AllowUserToDeleteRows = true;
                        }
                    }

                    else
                    {
                        DialogResult d;
                        d = MessageBox.Show("El navegador está cerrado, no se puede enviar mensajes!, conecte otra vez presionando <Conectar WhatsApp>", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);



                    }

                }




            }
            else
            {
                MessageBox.Show("No cuenta con acceso a internet, no puedes continuar.", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }





        }

        private async Task Excecutesendtask2()
        {

            if (CheckForInternetConnection())
            {

                //condicionales y token de cancellation



                string actualnumber = "";
                string actualname = "";




                //variables


                stopbtnclicked2 = false;
                rowcount2 = Convert.ToInt32(contacts2dgv.RowCount) - 1;
                send2pbr.Value = 0;
                send2pbr.Maximum = rowcount2;
                totalmessages2lbl.Text = rowcount2.ToString();
                int count = 0;

                sendedmessage2 = 0;
                notsendedmessage2 = 0;




                notsendedmessage2lbl.Text = sendedmessage2.ToString();
                sendedmessage2lbl.Text = notsendedmessage2.ToString();





                if (contacts2dgv.Rows.Count < 2) { MessageBox.Show("No existen contactos a los que enviar SMS!", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else
                {


                    if (sms1txt.Text == "") { MessageBox.Show("No existe mensaje para enviar SMS!.\nUtilice el espacio de <Mensaje 1>", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    else
                    {





                        if (wa.IsBrowserClosed(WA.driver2) == false)
                        {
                            if (!wa.IfConnected2(By.XPath("//div[contains(text(),'Iniciar chat')]")))
                            {
                                DialogResult d;
                                d = MessageBox.Show("Debe escanear el codigo QR para empezar a enviar SMS", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);


                            }


                            else
                            {

                                start2btn.Enabled = false;
                                pause2btn.Enabled = true;
                                stop2btn.Enabled = true;
                                pausetiming = 0;
                                logout2btn.Enabled = false;

                                connectgoobtn.Enabled = false;


                                int countmessage = 0;
                                loadmessage2lbl.Text = "Estado: Conectado . . .";
                                bool activatemanymessages = false;
                                contacts2dgv.AllowUserToAddRows = false;
                                contacts2dgv.AllowUserToDeleteRows = false;



                                if (manymessages2cb.Checked == true)
                                {
                                    if (sms2txt.Text == "" || sms3txt.Text == "" || sms4txt.Text == "" || sms5txt.Text == "")
                                    {
                                        manymessages2cb.Checked = false;
                                        MessageBox.Show("No llenó todos los espacios de mensajes, no se usará la opción <Enviar varios textos en un solo envío>", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);



                                    }
                                }



                                foreach (DataGridViewRow fila in contacts2dgv.Rows)
                                {


                                    if (fila.IsNewRow) continue;




                                    if (CheckForInternetConnection())
                                    {

                                        if (preventblock2cb.Checked == true)
                                        {
                                            wa.preventblocktiming2 = 4000;
                                        }
                                        else
                                        {
                                            wa.preventblocktiming2 = 0;
                                        }




                                        if (eachmessagetiming2cb.Checked == true)
                                        {
                                            eachmessagetiming2 = Convert.ToInt32(eachmessagetiming2txt.Text) * 1000;
                                        }
                                        else
                                        {
                                            eachmessagetiming2 = 0;
                                        }







                                        if (Convert.ToString(fila.Cells[0].Value) != string.Empty)
                                        { actualnumber = Convert.ToString(fila.Cells[0].Value); Console.WriteLine(actualnumber); }
                                        else { actualnumber = "numero vacio"; Console.WriteLine(actualnumber); }



                                        if (Convert.ToString(fila.Cells[1].Value) != string.Empty)
                                        { actualname = Convert.ToString(fila.Cells[1].Value); Console.WriteLine(actualname); }
                                        else { actualname = "nombre vacio"; Console.WriteLine(actualname); }




                                        //mensajes

                                        string m1 = Regex.Replace(sms1txt.Text, "\n", "\\n");
                                        string m2 = Regex.Replace(sms1txt.Text, "\n", "\\n");
                                        string m3 = Regex.Replace(sms1txt.Text, "\n", "\\n");
                                        string m4 = Regex.Replace(sms1txt.Text, "\n", "\\n");
                                        string m5 = Regex.Replace(sms1txt.Text, "\n", "\\n");

                                        var messages = new List<string>
                                        {
                                            m1,
                                            m2,
                                            m3,
                                            m4,
                                            m5
                                        };



                                        string actualmessagetosend = "";


                                        //bucle de mensajes 2, 3, 4 , 5 yremplazo con regards y goodbyes



                                        if (manymessages2cb.Checked == true && activatemanymessages == true)
                                        {
                                            countmessage++;




                                            if (countmessage == 5)
                                            {
                                                countmessage = 0;

                                                actualmessagetosend = actualmessagetosend + messages[0];


                                                if (sendfullname2cb.Checked == true || actualmessagetosend.Contains("{nombre}"))
                                                {

                                                    if (actualname == "nombre vacio")
                                                    {
                                                        actualmessagetosend = Regex.Replace(actualmessagetosend, "{nombre}", "");
                                                    }

                                                    else
                                                    {
                                                        actualmessagetosend = Regex.Replace(actualmessagetosend, "{nombre}", actualname);
                                                    }

                                                }
                                                if (senddatetime2cb.Checked || actualmessagetosend.Contains("{fecha}"))
                                                {
                                                    DateTime actualdate = getTimeNow();

                                                    actualmessagetosend = Regex.Replace(actualmessagetosend, "{fecha}", Convert.ToString(actualdate));

                                                }




                                                activatemanymessages = false;



                                            }
                                            else
                                            {


                                                actualmessagetosend = actualmessagetosend + messages[countmessage];

                                                if (sendfullname2cb.Checked || actualmessagetosend.Contains("{nombre}"))
                                                {

                                                    if (actualname == "nombre vacio")
                                                    {
                                                        actualmessagetosend = Regex.Replace(actualmessagetosend, "{nombre}", "");
                                                    }

                                                    else
                                                    {
                                                        actualmessagetosend = Regex.Replace(actualmessagetosend, "{nombre}", actualname);
                                                    }

                                                }
                                                if (senddatetime2cb.Checked || actualmessagetosend.Contains("{fecha}"))
                                                {
                                                    DateTime actualdate = getTimeNow();

                                                    actualmessagetosend = Regex.Replace(actualmessagetosend, "{fecha}", Convert.ToString(actualdate));

                                                }


                                            }

                                        }
                                        else
                                        {



                                            actualmessagetosend = actualmessagetosend + messages[0];


                                            if (sendfullname2cb.Checked || actualmessagetosend.Contains("{nombre}"))
                                            {

                                                if (actualname == "nombre vacio")
                                                {

                                                    actualmessagetosend = Regex.Replace(actualmessagetosend, "{nombre}", "");

                                                }

                                                else
                                                {
                                                    actualmessagetosend = Regex.Replace(actualmessagetosend, "{nombre}", actualname);
                                                }

                                            }

                                            if (senddatetime2cb.Checked || actualmessagetosend.Contains("{fecha}"))
                                            {
                                                DateTime actualdate = getTimeNow();

                                                actualmessagetosend = Regex.Replace(actualmessagetosend, "{fecha}", Convert.ToString(actualdate));

                                            }


                                        }


                                        try
                                        {



                                            loadmessage2lbl.Text = "";
                                            loadmessage2lbl.Text = "Estado: Conectado . . .";




                                            if (actualnumber != "numero vacio")
                                            {
                                                Console.WriteLine("el numero no esta vacio y paso a busca contacto en SMS");



                                                Console.WriteLine("entre a escribir");


                                                await Task.Run(() =>
                                                {


                                                    if (pausetiming2 != 0)
                                                    {
                                                        try
                                                        {
                                                            pausetimingaction(pausetiming2, pauseToken2.Token);
                                                            pausetiming2 = 0;
                                                        }
                                                        catch (Exception ex)
                                                        {

                                                            Console.WriteLine(ex.Message);
                                                        }

                                                    }


                                                    try
                                                    {



                                                        Actions action = new Actions(WA.driver2);


                                                        wa.ClickSearchIcon2();



                                                        //WA.driver2.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

                                                        action.SendKeys(Keys.Space).Build().Perform();

                                                        wa.ContactSearch2(actualnumber);





                                                        // WA.driver2.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

                                                        // Task.Delay(2000).Wait();

                                                        Console.WriteLine("doy click en el contacto");

                                                        wa.ContactClick2();



                                                        //WA.driver2.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);



                                                        Task.Delay(2000).Wait();


                                                        //Console.WriteLine(actualmessagetosend + "MESSAGEEEEEEEE SMS");


                                                        wa.ContactMessage2(actualmessagetosend);



                                                        Console.WriteLine("solo Mensaje escrito");

                                                        Task.Delay(1000 + wa.preventblocktiming2).Wait();

                                                        wa.ContactActionEnter2();

                                                        Console.WriteLine("presione enter para enviar");

                                                        fila.Cells[2].Value = "S";




                                                        Task.Delay(1000 + wa.preventblocktiming2).Wait();

                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);

                                                    }


                                                }, cancellationToken2.Token);






                                                sendedmessage2++;
                                                sendedmessage2lbl.Text = Convert.ToString(sendedmessage2);
                                                Console.WriteLine(sendedmessage2);

                                            }
                                            else
                                            {
                                                fila.Cells[2].Value = "N";
                                                notsendedmessage2++;
                                                notsendedmessage2lbl.Text = Convert.ToString(notsendedmessage2);
                                                Console.WriteLine(notsendedmessage2);
                                            }


                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message.ToString());


                                        }
                                    }

                                    else
                                    {


                                        stop2btn.Enabled = false;
                                        pause2btn.Enabled = false;
                                        start2btn.Enabled = true;
                                        MessageBox.Show("Se detuvieron los envios de SMS debido a que no cuenta con acceso a internet.", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                        break;
                                    }

                                    count++;
                                    if (count <= rowcount2)
                                    {
                                        send2pbr.Value = count;
                                    }

                                    activatemanymessages = true;


                                    if (severalpause2txt.Text != "")
                                    {


                                        if (fila.Index == Convert.ToInt32(severalpause2txt.Text) && !severalpausetoken2.IsCancellationRequested)
                                        {

                                            Console.WriteLine("<<<<<<<<<<<<<<<<<<<este es la cuenta ctual de la fila  " + fila.Index);
                                            MessageBox.Show("El envio se pausó debido al <# mensajes para Pausar> designado en esta sección.\nRecomendamos esta pausa para no ser bloqueado en WhatsApp.\nLa pausa suele durar 15 minutos y se empezo el <" + getTimeNow() + ">, actualmente se pausa cada " + severalpausetxt.Text + " mensajes", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                            await Task.Run(() =>
                                            {
                                                try
                                                {
                                                    Task.Delay(TimeSpan.FromSeconds(900), severalpausetoken2.Token).Wait();

                                                }
                                                catch (Exception ex)
                                                {

                                                    Console.WriteLine(ex.Message);
                                                }


                                            });



                                        }



                                    }

                                    await Task.Run(() =>
                                    {
                                        try
                                        {
                                            Task.Delay(eachmessagetiming2, eachmessagetoken2.Token).Wait();


                                        }
                                        catch (Exception ex)
                                        {

                                            Console.WriteLine(ex.Message.ToString());
                                        }




                                    });




                                }



                                if (stopbtnclicked2 != true)
                                {
                                    MessageBox.Show("SMS enviados correctamente! ", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);


                                    stop2btn.Enabled = false;
                                    pause2btn.Enabled = false;
                                    start2btn.Enabled = true;
                                    logout2btn.Enabled = true;
                                    connectgoobtn.Enabled = true;

                                }
                                else
                                {
                                    start2btn.Enabled = true;
                                }

                                notsendedmessage2lbl.Text = Convert.ToString(rowcount2 - sendedmessage2);

                                contacts2dgv.AllowUserToAddRows = true;
                                contacts2dgv.AllowUserToDeleteRows = true;
                            }
                        }

                        else
                        {
                            DialogResult d;
                            d = MessageBox.Show("El navegador está cerrado, no se puede enviar mensajes!, conecte otra vez presionando <Conectar WhatsApp>", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);



                        }

                    }
                }




            }
            else
            {
                MessageBox.Show("No cuenta con acceso a internet, no puedes continuar.", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }





        }

        private void pastedatabtn_Click(object sender, EventArgs e)
        {

            maintab.SelectedTab = contactlisttab;
            try
            {
                contactsdgv.Rows.Clear();
                contactsdgv.Refresh();

                string s = Clipboard.GetText();

                string[] lines = s.Replace("\n", "").Split('\r');

                contactsdgv.Rows.Add(lines.Length - 1);
                string[] fields;
                int row = 0;
                int col = 0;

                foreach (string item in lines)
                {
                    fields = item.Split('\t');
                    foreach (string f in fields)
                    {

                        contactsdgv[col, row].Value = f;
                        col++;
                    }
                    row++;
                    col = 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("pegar 2 COLUMNAS (NUMERO, NOMBRE DE CONTACTO) de EXCEL", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }




        }

        private void Storecontaacts()
        {

            ToCsV(contactsdgv, "C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\datac.csv");
            ToCsV(contacts2dgv, "C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\datac2.csv");



        }

        private void Storemessages()
        {


            string path = "C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt";
            DirectoryInfo di = Directory.CreateDirectory(path);


            File.WriteAllText(path + "\\m1.txt", m1txt.Text);
            File.WriteAllText(path + "\\m2.txt", m2txt.Text);
            File.WriteAllText(path + "\\m3.txt", m3txt.Text);
            File.WriteAllText(path + "\\m4.txt", m4txt.Text);
            File.WriteAllText(path + "\\m5.txt", m5txt.Text);

            File.WriteAllText(path + "\\sms1.txt", sms1txt.Text);
            File.WriteAllText(path + "\\sms2.txt", sms2txt.Text);
            File.WriteAllText(path + "\\sms3.txt", sms3txt.Text);
            File.WriteAllText(path + "\\sms4.txt", sms4txt.Text);
            File.WriteAllText(path + "\\sms5.txt", sms5txt.Text);





        }

        private void Restoremessages()
        {




            if (File.Exists("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\m1.txt"))
            {
                m1txt.Text = File.ReadAllText("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\m1.txt");
            }
            if (File.Exists("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\m2.txt"))
            {
                m2txt.Text = File.ReadAllText("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\m2.txt");
            }
            if (File.Exists("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\m3.txt"))
            {
                m3txt.Text = File.ReadAllText("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\m3.txt");
            }
            if (File.Exists("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\m4.txt"))
            {
                m4txt.Text = File.ReadAllText("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\m4.txt");
            }
            if (File.Exists("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\m5.txt"))
            {
                m5txt.Text = File.ReadAllText("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\m5.txt");
            }




        }
        private void Restoremessages2()
        {

            if (File.Exists("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\sms1.txt"))
            {
                sms1txt.Text = File.ReadAllText("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\sms1.txt");
            }
            if (File.Exists("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\sms2.txt"))
            {
                sms2txt.Text = File.ReadAllText("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\sms2.txt");
            }
            if (File.Exists("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\sms3.txt"))
            {
                sms3txt.Text = File.ReadAllText("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\sms3.txt");
            }
            if (File.Exists("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\sms4.txt"))
            {
                sms4txt.Text = File.ReadAllText("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\sms4.txt");
            }
            if (File.Exists("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\sms5.txt"))
            {
                sms5txt.Text = File.ReadAllText("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\sms5.txt");
            }




        }

        private void ToCsV(DataGridView dGV, string filename)
        {


            string path = "C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt";
            DirectoryInfo di = Directory.CreateDirectory(path);

            string value = "";
            DataGridViewRow dr = new DataGridViewRow();
            StreamWriter swOut = new StreamWriter(filename);

            //write header rows to csv
            for (int i = 0; i <= dGV.Columns.Count - 2; i++)
            {
                if (i > 0)
                {
                    swOut.Write(";");
                }
                swOut.Write(dGV.Columns[i].HeaderText);
            }

            swOut.WriteLine();

            //write DataGridView rows to csv
            for (int j = 0; j <= dGV.Rows.Count - 2; j++)
            {
                if (j > 0)
                {
                    swOut.WriteLine();
                }

                dr = dGV.Rows[j];

                for (int i = 0; i <= dGV.Columns.Count - 2; i++)
                {
                    if (i > 0)
                    {
                        swOut.Write(";");
                    }


                    value = Convert.ToString(dr.Cells[i].Value);

                    //replace comma's with spaces
                    value = value.Replace(';', ' ');
                    //replace embedded newlines with spaces
                    value = value.Replace(Environment.NewLine, " ");

                    swOut.Write(value);
                }
            }
            swOut.Close();






        }

        public async Task ReadCSV()
        {

            if (File.Exists("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\datac.csv"))
            {

                using (var reader = new StreamReader("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\datac.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                {

                    using (var dr = new CsvDataReader(csv))
                    {

                        contactsdgv.Columns.Clear();

                        DataTable dtbl = new DataTable();


                        await Task.Run(() =>
                        {
                            dtbl.Load(dr);
                        });
                        contactsdgv.Columns.Add("Column", "Numero o Grupo");
                        contactsdgv.Columns.Add("Column", "Nombre");
                        contactsdgv.Columns.Add("Column", "Enviado (S/N)");




                        for (int i = 0; i < dtbl.Rows.Count; i++)
                        {

                            contactsdgv.Rows.Add(Convert.ToString(dtbl.Rows[i]["Numero o Grupo"]), Convert.ToString(dtbl.Rows[i]["Nombre"]));


                        }
                        DataGridViewColumn column = contactsdgv.Columns[0];
                        column.Width = 200;



                        DataGridViewColumn column1 = contactsdgv.Columns[1];
                        column1.Width = 350;




                        DataGridViewColumn column2 = contactsdgv.Columns[2];
                        column2.Width = 100;
                        column2.ReadOnly = true;









                    }
                }

            }


        }
        public async Task ReadCSV2()
        {

            if (File.Exists("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\datac2.csv"))
            {

                using (var reader = new StreamReader("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\datac2.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                {

                    using (var dr = new CsvDataReader(csv))
                    {

                        contacts2dgv.Columns.Clear();

                        DataTable dtbl = new DataTable();


                        await Task.Run(() =>
                        {
                            dtbl.Load(dr);
                        });
                        contacts2dgv.Columns.Add("Column", "Numero o Grupo");
                        contacts2dgv.Columns.Add("Column", "Nombre");
                        contacts2dgv.Columns.Add("Column", "Enviado (S/N)");




                        for (int i = 0; i < dtbl.Rows.Count; i++)
                        {

                            contacts2dgv.Rows.Add(Convert.ToString(dtbl.Rows[i]["Numero o Grupo"]), Convert.ToString(dtbl.Rows[i]["Nombre"]));


                        }
                        DataGridViewColumn column = contacts2dgv.Columns[0];
                        column.Width = 200;



                        DataGridViewColumn column1 = contacts2dgv.Columns[1];
                        column1.Width = 350;




                        DataGridViewColumn column2 = contacts2dgv.Columns[2];
                        column2.Width = 100;
                        column2.ReadOnly = true;









                    }
                }

            }


        }


        private void gmailbtn_Click(object sender, EventArgs e)
        {
            cmsgmail.Show(Cursor.Position.X, Cursor.Position.Y);
        }

        private void expotarDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportDgvToGmail();
        }

        private void importarDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportGmailToDgv();


        }

        private void clearfilenamebtn_Click(object sender, EventArgs e)
        {
            filenametxt.Clear();

        }

        private void savebtn_Click(object sender, EventArgs e)
        {




            if (contactsdgv.Rows.Count > 1)
            {


                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Archivo de Texto (*.txt)|*.txt",
                    FileName = ""
                };
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("No fue posible escribir datos en el disco." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {

                            string value = "";


                            DataGridViewRow dr = new DataGridViewRow();
                            StreamWriter swOut = new StreamWriter(sfd.FileName);



                            //write DataGridView rows to csv
                            for (int j = 0; j <= contactsdgv.Rows.Count - 2; j++)
                            {
                                if (j > 0)
                                {
                                    swOut.WriteLine();
                                }

                                dr = contactsdgv.Rows[j];

                                for (int i = 0; i <= contactsdgv.Columns.Count - 2; i++)
                                {
                                    if (i > 0)
                                    {
                                        swOut.Write("\t");
                                    }
                                    if (i < 1)
                                    {
                                        if (Convert.ToString(dr.Cells[i].Value).Replace(" ", "").Length > 9)
                                        {

                                            if (Convert.ToString(dr.Cells[i].Value).StartsWith("+") == false && IsDigitsOnly(Convert.ToString(dr.Cells[i].Value)))
                                            {
                                                swOut.Write("+");
                                            }



                                        }

                                    }

                                    value = Convert.ToString(dr.Cells[i].Value);


                                    //replace comma's with spaces
                                    value = value.Replace('\t', ' ');
                                    //replace embedded newlines with spaces
                                    value = value.Replace(Environment.NewLine, " ");

                                    swOut.Write(value);
                                }
                            }
                            swOut.Close();
                            MessageBox.Show("Datos exportados correctamente!", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay datos a exportar", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }




        }

        private void openbtn_Click(object sender, EventArgs e)
        {

            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "Archivo de Texto (*.txt)|*.txt";
            sfd.FileName = sfd.FileName;


            if (sfd.ShowDialog() == DialogResult.OK)
            {

                if (File.Exists(sfd.FileName))
                {




                    StreamReader sr = new StreamReader(sfd.FileName);
                    StringBuilder sb = new StringBuilder();


                    string s;

                    contactsdgv.Columns.Clear();


                    contactsdgv.Columns.Add("Column", "Numero o Grupo");
                    contactsdgv.Columns.Add("Column", "Nombre");
                    contactsdgv.Columns.Add("Column", "Enviado (S/N)");

                    while (!sr.EndOfStream)
                    {
                        s = sr.ReadLine();

                        string[] str = s.Split('\t');



                        contactsdgv.Rows.Add(str[0].ToString(), str[1].ToString());


                    }
                    sr.Close();

                    DataGridViewColumn column = contactsdgv.Columns[0];
                    column.Width = 200;



                    DataGridViewColumn column1 = contactsdgv.Columns[1];
                    column1.Width = 350;




                    DataGridViewColumn column2 = contactsdgv.Columns[2];
                    column2.Width = 100;
                    column2.ReadOnly = true;

                    MessageBox.Show("Datos importados!", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    maintab.SelectedTab = contactlisttab;

                }

            }





        }

        private void minutosToolStripMenuItem_Click(object sender, EventArgs e)
        {



            if (pausetiming == 0)
            {
                pauseToken = new CancellationTokenSource();

                pausetiming = 300;
                pausebtn.Text = "Reanudar";
                MessageBox.Show("Los envíos se pausarán en breve.", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logoutbtn.Enabled = true;
            }



        }

        private void minutosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pauseToken = new CancellationTokenSource();

            if (pausetiming == 0)
            {
                pausetiming = 1800;
                pausebtn.Text = "Reanudar";
                MessageBox.Show("Los envíos se pausarán en breve.", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logoutbtn.Enabled = true;
            }


        }

        private void horaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pauseToken = new CancellationTokenSource();
            if (pausetiming == 0)
            {
                pausetiming = 3600;
                pausebtn.Text = "Reanudar";
                MessageBox.Show("Los envíos se pausarán en breve.", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logoutbtn.Enabled = true;
            }

        }

        private void horaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pauseToken = new CancellationTokenSource();
            if (pausetiming == 0)
            {
                pausetiming = 7200;
                pausebtn.Text = "Reanudar";
                MessageBox.Show("Los envíos se pausarán en breve.", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logoutbtn.Enabled = true;
            }


        }

        private void pausetimingaction(Int32 pauset, CancellationToken token)
        {
            try
            {
                if (pauset > 0)
                {

                    MessageBox.Show(String.Format("Se pausarán los envios por {0} minutos", pauset / 60), "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Task.Delay(TimeSpan.FromSeconds(pauset), token).Wait();




                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }


        }

        private void ExecuteStart()
        {



            try
            {


                detailtimer.Start();

                UserModel user = new UserModel();




                OpenSaved();
                OpenSaved2();

                controls(false);
                controls2(false);


                pausebtn.Enabled = false;
                stopbtn.Enabled = false;

                pause2btn.Enabled = false;
                stop2btn.Enabled = false;

                sendpbr.Value = 0;
                send2pbr.Value = 0;


                logoutbtn.Enabled = false;
                logout2btn.Enabled = false;




                OpenSettings();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }


        }

        private void CheckUserProfileExist()
        {


            if (!Directory.Exists("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\Chrome WA Profile\\"))
            {

                try
                {

                    string path = "C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\";
                    DirectoryInfo di = Directory.CreateDirectory(path);





                    using (WebClient Client = new WebClient())
                    {

                        Client.DownloadFile(chromewadefaultuserdata, "C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\Chrome WA Profile.zip");







                        var zipFileName = "C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\Chrome WA Profile.zip";

                        var targetDir = "C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\";


                        FastZip fastZip = new FastZip();
                        string fileFilter = null;

                        // Will always overwrite if target filenames already exist
                        fastZip.ExtractZip(zipFileName, targetDir, fileFilter);


                        File.Delete("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\Chrome WA Profile.zip");




                    }


                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }









            }


            if (!Directory.Exists("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\Chrome SMS Profile\\"))
            {

                try
                {

                    string path = "C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\";
                    DirectoryInfo di = Directory.CreateDirectory(path);





                    using (WebClient Client = new WebClient())
                    {

                        Client.DownloadFile(chromesmsdefaultuserdata, "C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\Chrome SMS Profile.zip");





                        var zipFileName = "C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\Chrome SMS Profile.zip";

                        var targetDir = "C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\";


                        FastZip fastZip = new FastZip();
                        string fileFilter = null;

                        // Will always overwrite if target filenames already exist
                        fastZip.ExtractZip(zipFileName, targetDir, fileFilter);


                        File.Delete("C:\\Users\\" + actualuser + "\\Documents\\tempfilesWAButt\\Chrome SMS Profile.zip");




                    }


                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }









            }





        }
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
        private async void logoutbtn_Click(object sender, EventArgs e)
        {

            if (CheckForInternetConnection())
            {
                await Task.Run(() =>
                {
                    try
                    {
                        wa.LogoutWA();
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }



                });

                DialogResult d;
                d = MessageBox.Show("¿Desea cerrar el programa?", "Observación", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                if (d == DialogResult.Yes)
                {
                    try
                    {

                        Storecontaacts();
                        Storemessages();
                        UserModel user = new UserModel();
                        logoutbtn.Enabled = false;
                        wa.CloseWDriver();
                        WAButtfrm.ActiveForm.Text = "WAButt";
                        Application.Exit();


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }





            }
            else
            {
                MessageBox.Show("No cuenta con acceso a internet, no puede continuar.", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
        private void manymessagescb_Click(object sender, EventArgs e)
        {

            if (manymessagescb.Checked == true)
            {
                if (m2txt.Text == "" || m3txt.Text == "" || m4txt.Text == "" || m5txt.Text == "")
                {
                    MessageBox.Show("Debe llenar todos los espacios para mensajes! para usar esta opcion", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    manymessagescb.Checked = false;


                }

            }

        }
        private static DateTime getTimeNow()
        {
            /*
                var client = new TcpClient("time.nist.gov", 13);
                using (var streamReader = new StreamReader(client.GetStream()))
                {



                    var response = streamReader.ReadToEnd();
                    var utcDateTimeString = response.Substring(7, 17);
                    var localDateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

                    return localDateTime;



                }
                */



            DateTime localDate = DateTime.Now;
            String cultureName = "es-PE";


            var culture = new CultureInfo(cultureName);
            string res = localDate.ToString(culture);

            return Convert.ToDateTime(res);


        }
        private void getcontactsfromgroupbtn_Click(object sender, EventArgs e)
        {
            // wa.GetContactsFromGroup();
        }
        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;

            }

            return true;
        }
        private void severalpausetxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            InputNumbers(sender, e);
        }
        private void eachmessagetimingtxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            InputNumbers(sender, e);
        }
        private void extractgroupnumbersbtn_Click(object sender, EventArgs e)
        {
            ExtractContacts ext = new ExtractContacts(this);
            ext.ShowDialog();

        }


        public async Task GetContactsFromGroup(string tosearch)
        {

            StringBuilder str = new StringBuilder();
            strex = new StringBuilder();
            await Task.Run(() =>
            {

                str = wa.GetContactsFromGroup(tosearch);

                if (Convert.ToString(str) != "")
                {
                    string[] words = WAButtfrm.GetWords(tosearch);
                    string converted = "";

                    foreach (var item in words)
                    {
                        converted = converted + item;
                    }


                    try
                    {
                        filenameextracted = Path.Combine(Environment.GetFolderPath(
                       Environment.SpecialFolder.ApplicationData), "wabutt" + converted + DateTime.Now.ToString("dd-MM-yyyy") + ".csv");


                        StreamWriter swOut = new StreamWriter(filenameextracted);

                        strex.Append(str.ToString().Replace(", ", "\n,").Replace("\n,Tú", "").Replace("\n,You", ""));
                        swOut.Write(str.ToString().Replace(", ", "\n,").Replace("\n,Tú", "").Replace("\n,You", ""));

                        swOut.Close();



                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error :" + ex.Message);
                    }

                }





            });






            //Console.WriteLine(Regex. Replace(converted, @"[^\u0000-\u007F]+", ""));




        }
        public static DataTable ReadCSV3(string path)
        {
            DataTable dt = new DataTable();



            if (File.Exists(path))
            {
                Console.WriteLine("READCSV3 OPEN");

                StreamReader sr = new StreamReader(path);
                StringBuilder sb = new StringBuilder();


                string s;


                dt.Columns.Add("Numero", typeof(string));



                int indexname;


                s = sr.ReadLine();

                string[] strs = s.Split(',');


                indexname = strs.ToList().IndexOf("Mobile Phone");




                while (!sr.EndOfStream)
                {
                    s = sr.ReadLine();

                    string[] str = s.Split(',');


                    //because the first line is header
                    string str1 = str[0].ToString();


                    if (!str1.Equals("First Name"))
                    {

                        DataRow row = dt.NewRow();

                        row["Numero"] = str[indexname].ToString();

                        dt.Rows.Add(row);



                    }
                }

                sr.Close();




            }


            return dt;

        }
        public static void StoreGroupContacts(string converted, StringBuilder str)
        {

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Archivo CSV (*.csv)|*.csv",
                FileName = converted

            };

            bool fileError = false;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(sfd.FileName))
                {
                    try
                    {
                        File.Delete(sfd.FileName);
                    }
                    catch (IOException ex)
                    {
                        fileError = true;
                        MessageBox.Show("No fue posible escribir datos en el disco." + ex.Message);
                    }
                }
                if (!fileError)
                {
                    try
                    {


                        StreamWriter swOut = new StreamWriter(sfd.FileName);


                        swOut.Write(str.ToString().Replace(", ", "\n,"));

                        swOut.Close();
                        MessageBox.Show("Datos exportados correctamente!", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error :" + ex.Message);
                    }
                }
            }

        }
        public static string[] GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select TrimSuffix(m.Value);

            return words.ToArray();
        }
        public static string TrimSuffix(string word)
        {
            int apostropheLocation = word.IndexOf('\'');
            if (apostropheLocation != -1)
            {
                word = word.Substring(0, apostropheLocation);
            }

            return word;
        }


        private void Extracttimer_Tick(object sender, EventArgs e)
        {
            if (startbtn.Enabled)
            {
                extractgroupnumbersbtn.Visible = true;

                extractlbl.Visible = true;


                severalpauselbl.Location = new Point(720, 60);
                severalpausetxt.Location = new Point(755, 22);



            }
            else
            {
                extractgroupnumbersbtn.Visible = false; extractlbl.Visible = false;

                severalpauselbl.Location = new Point(605, 60);
                severalpausetxt.Location = new Point(640, 22);
            }



            if (apptab.SelectedTab == wabottab)
            {
                colorpanel.BackColor = Color.FromArgb(8, 112, 100);
            }

            else
            {
                colorpanel.BackColor = Color.FromArgb(19, 116, 233);
            }



        }
        private async void connectgoobtn_Click(object sender, EventArgs e)
        {

            if (CheckForInternetConnection())
            {


                wa.CloseWDriver2();



                try
                {
                    loadmessage2lbl.Text = "Estado: Conectando . . . ";


                    await wa.LaunchBrowser2();


                    loadmessage2lbl.Text = "Estado: Navegador Abierto, escanee código QR o empiece a enviar";



                    controls2(true);
                    logout2btn.Enabled = true;




                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error :" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                MessageBox.Show("No cuenta con acceso a internet, no puedes continuar.", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void selectwabtn_Click(object sender, EventArgs e)
        {
            apptab.SelectedTab = wabottab;


        }
        private void selectsmsbtn_Click(object sender, EventArgs e)
        {
            apptab.SelectedTab = smsbottab;


        }
        private void pastedata2btn_Click(object sender, EventArgs e)
        {
            main2tab.SelectedTab = contactlist2tab;
            try
            {
                contacts2dgv.Rows.Clear();
                contacts2dgv.Refresh();

                string s = Clipboard.GetText();

                string[] lines = s.Replace("\n", "").Split('\r');

                contacts2dgv.Rows.Add(lines.Length - 1);
                string[] fields;
                int row = 0;
                int col = 0;

                foreach (string item in lines)
                {
                    fields = item.Split('\t');
                    foreach (string f in fields)
                    {

                        contacts2dgv[col, row].Value = f;
                        col++;
                    }
                    row++;
                    col = 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Solo se pueden pegar 2 COLUMNAS (NUMERO, NOMBRE DE CONTACTO) de EXCEL", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void save2btn_Click(object sender, EventArgs e)
        {


            if (contacts2dgv.Rows.Count > 1)
            {


                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Archivo de Texto (*.txt)|*.txt",
                    FileName = ""
                };
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("No fue posible escribir datos en el disco." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {

                            string value = "";


                            DataGridViewRow dr = new DataGridViewRow();
                            StreamWriter swOut = new StreamWriter(sfd.FileName);



                            //write DataGridView rows to csv
                            for (int j = 0; j <= contacts2dgv.Rows.Count - 2; j++)
                            {
                                if (j > 0)
                                {
                                    swOut.WriteLine();
                                }

                                dr = contacts2dgv.Rows[j];

                                for (int i = 0; i <= contacts2dgv.Columns.Count - 2; i++)
                                {
                                    if (i > 0)
                                    {
                                        swOut.Write("\t");
                                    }
                                    if (i < 1)
                                    {
                                        if (Convert.ToString(dr.Cells[i].Value).Replace(" ", "").Length > 9)
                                        {

                                            if (Convert.ToString(dr.Cells[i].Value).StartsWith("+") == false && IsDigitsOnly(Convert.ToString(dr.Cells[i].Value)))
                                            {
                                                swOut.Write("+");
                                            }



                                        }

                                    }

                                    value = Convert.ToString(dr.Cells[i].Value);


                                    //replace comma's with spaces
                                    value = value.Replace('\t', ' ');
                                    //replace embedded newlines with spaces
                                    value = value.Replace(Environment.NewLine, " ");

                                    swOut.Write(value);
                                }
                            }
                            swOut.Close();
                            MessageBox.Show("Datos exportados correctamente!", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay datos a exportar", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
        private void open2btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "Archivo de Texto (*.txt)|*.txt";
            sfd.FileName = sfd.FileName;


            if (sfd.ShowDialog() == DialogResult.OK)
            {

                if (File.Exists(sfd.FileName))
                {




                    StreamReader sr = new StreamReader(sfd.FileName);
                    StringBuilder sb = new StringBuilder();


                    string s;

                    contacts2dgv.Columns.Clear();


                    contacts2dgv.Columns.Add("Column", "Numero o Grupo");
                    contacts2dgv.Columns.Add("Column", "Nombre");
                    contacts2dgv.Columns.Add("Column", "Enviado (S/N)");

                    while (!sr.EndOfStream)
                    {
                        s = sr.ReadLine();

                        string[] str = s.Split('\t');



                        contacts2dgv.Rows.Add(str[0].ToString(), str[1].ToString());


                    }
                    sr.Close();

                    DataGridViewColumn column = contacts2dgv.Columns[0];
                    column.Width = 200;



                    DataGridViewColumn column1 = contacts2dgv.Columns[1];
                    column1.Width = 350;




                    DataGridViewColumn column2 = contacts2dgv.Columns[2];
                    column2.Width = 100;
                    column2.ReadOnly = true;

                    MessageBox.Show("Datos importados!", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    main2tab.SelectedTab = contactlist2tab;

                }

            }
        }
        private void emoji2btn_Click(object sender, EventArgs e)
        {
            Process.Start("https://es.piliapp.com/twitter-symbols/");
        }
        private void gmail2btn_Click(object sender, EventArgs e)
        {
            cmsgmail2.Show(Cursor.Position.X, Cursor.Position.Y);
        }
        private void importarDatosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ImportGmailToDgv2();
        }
        private void exportarDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportDgvToGmail2();
        }
        private async void start2btn_Click(object sender, EventArgs e)
        {



            cancellationToken2 = new CancellationTokenSource();
            pauseToken2 = new CancellationTokenSource();
            eachmessagetoken2 = new CancellationTokenSource();
            severalpausetoken2 = new CancellationTokenSource();
            await Excecutesendtask2();





        }

        private void pause2btn_Click(object sender, EventArgs e)
        {

            if (pausetiming2 > 0)
            {
                if (pauseToken2 != null)
                {
                    pause2btn.Text = "Pausar";
                    pauseToken2.Cancel();
                    pause2btn.Enabled = true;
                    stop2btn.Enabled = true;
                    start2btn.Enabled = false;
                    logout2btn.Enabled = false;
                    connectgoobtn.Enabled = true;



                }

                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("No cuenta con acceso a internet, recomendamos esperar hasta tener conexión para continuar.", "Sugerencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }

            else
            {
                cmspause2.Show(Cursor.Position.X, Cursor.Position.Y);
            }



        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {


            if (pausetiming2 == 0)
            {
                pauseToken2 = new CancellationTokenSource();

                pausetiming2 = 300;
                pause2btn.Text = "Reanudar";
                MessageBox.Show("Los envíos SMS se pausarán en breve.", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logout2btn.Enabled = true;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            pauseToken2 = new CancellationTokenSource();

            if (pausetiming2 == 0)
            {
                pausetiming2 = 1800;
                pause2btn.Text = "Reanudar";
                MessageBox.Show("Los envíos SMS se pausarán en breve.", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logout2btn.Enabled = true;
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            pauseToken2 = new CancellationTokenSource();
            if (pausetiming2 == 0)
            {
                pausetiming2 = 3600;
                pause2btn.Text = "Reanudar";
                MessageBox.Show("Los envíos SMS se pausarán en breve.", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logout2btn.Enabled = true;
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            pauseToken2 = new CancellationTokenSource();
            if (pausetiming2 == 0)
            {
                pausetiming2 = 7200;
                pause2btn.Text = "Reanudar";
                MessageBox.Show("Los envíos SMS se pausarán en breve.", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logout2btn.Enabled = true;
            }
        }

        private void stop2btn_Click(object sender, EventArgs e)
        {
            if (pausetiming2 > 0)
            {
                DialogResult a;
                a = MessageBox.Show("Los envios SMS están pausados ¿Desea detener la cola de envios? ", "Observación", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (a == DialogResult.Yes)
                {

                    if (cancellationToken2 != null)
                    {


                        MessageBox.Show("Los envios SMS se detendrán en breve ", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        pauseToken2.Cancel();
                        cancellationToken2.Cancel();
                        pausetiming2 = 0;
                        pause2btn.Enabled = false;
                        stop2btn.Enabled = false;
                        eachmessagetoken2.Cancel();
                        severalpausetoken2.Cancel();
                        stopbtnclicked2 = true;
                        logout2btn.Enabled = true;
                        connectgoobtn.Enabled = true;
                        pause2btn.Text = "Pausar";




                        if (!CheckForInternetConnection())
                        {
                            MessageBox.Show("No cuenta con acceso a internet, recomendamos esperar hasta tener conexión para continuar.", "Sugerencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }

            }
            else
            {
                DialogResult b;
                b = MessageBox.Show("Desea detener los envíos SMS? ", "Observación", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (b == DialogResult.Yes)
                {

                    if (cancellationToken2 != null)
                    {
                        MessageBox.Show("Los envios se detendrán en breve ", "Observación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cancellationToken2.Cancel();
                        pausetiming2 = 0;
                        pause2btn.Enabled = false;
                        stop2btn.Enabled = false;
                        eachmessagetoken2.Cancel();
                        severalpausetoken2.Cancel();
                        stopbtnclicked2 = true;
                        logout2btn.Enabled = true;
                        connectgoobtn.Enabled = true;


                        pause2btn.Text = "Pausar";



                        if (!CheckForInternetConnection())
                        {
                            MessageBox.Show("No cuenta con acceso a internet, recomendamos esperar hasta tener conexión para continuar.", "Sugerencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }

                }

            }
        }

        public void ClearEmptyRows()
        {



            for (int i = contactsdgv.Rows.Count - 1; i > -1; i--)
            {
                DataGridViewRow row = contactsdgv.Rows[i];
                if (!row.IsNewRow && Convert.ToString(row.Cells[0].Value) == "")
                {
                    contactsdgv.Rows.RemoveAt(i);
                }
            }





        }

        private void clearemptyrowsbtn_Click(object sender, EventArgs e)
        {
            ClearEmptyRows();
        }

        private void deleteduplicatedbtn_Click(object sender, EventArgs e)
        {


        }
        private void DeleteDuplicate1()
        {
            DataTable items = new DataTable();

            items.Columns.Add("Numero o Grupo", typeof(string));
            items.Columns.Add("Nombre", typeof(string));
            items.Columns.Add("Enviado(S/N)", typeof(string));

            for (int i = 0; i < contactsdgv.Rows.Count; i++)
            {
                DataRow rw = items.NewRow();
                rw[0] = Convert.ToString(contactsdgv.Rows[i].Cells[0].Value);
                rw[1] = Convert.ToString(contactsdgv.Rows[i].Cells[1].Value);
                rw[2] = Convert.ToString(contactsdgv.Rows[i].Cells[2].Value);
                if (!items.Rows.Cast<DataRow>().Any(row => row["Numero o Grupo"].Equals(rw["Numero o Grupo"])))
                    items.Rows.Add(rw);
            }



            contactsdgv.Rows.Clear();


            foreach (DataRow item in items.Rows)
            {
                contactsdgv.Rows.Add(Convert.ToString(item[0]), Convert.ToString(item[1]), Convert.ToString(item[2]));
            }




            DataGridViewColumn column = contactsdgv.Columns[0];
            column.Width = 200;



            DataGridViewColumn column1 = contactsdgv.Columns[1];
            column1.Width = 350;




            DataGridViewColumn column2 = contactsdgv.Columns[2];
            column2.Width = 100;
            column2.ReadOnly = true;
        }

        private void dgvwacopymodecms_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendKeys.Send("^C");
        }

        private void contactsdgv_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                int currentMouseOverRow = contactsdgv.HitTest(e.X, e.Y).RowIndex;


                copyrowcms.Show(contactsdgv, new Point(e.X, e.Y));

            }
        }
        private string EncodeNonAsciiCharacters(string value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in value)
            {
                if (c > 127)
                {
                    // This character is too big for ASCII
                    string encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        private bool CheckAttachMessageStatus()
        {
            if (!CheckAttachMessageStatusSub() || sendonlyattachcb.Checked)
            {
                return false;
            }

            return true;
        }
        private bool CheckAttachMessageStatusSub()
        {
            if (!string.IsNullOrEmpty(filenametxt.Text)
                && string.IsNullOrWhiteSpace(m1txt.Text)
                && string.IsNullOrWhiteSpace(m2txt.Text)
                && string.IsNullOrWhiteSpace(m3txt.Text)
                && string.IsNullOrWhiteSpace(m4txt.Text)
                && string.IsNullOrWhiteSpace(m5txt.Text)



                )
            {
                return false;
            }

            return true;
        }


        private string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }
        private bool GetImageState(string path)
        {
            if (GetExtension(path) == ".svg" || GetExtension(path) == ".png" || GetExtension(path) == ".jpg"
                || GetExtension(path) == ".jpeg" || GetExtension(path) == ".ico" || GetExtension(path) == ".gif" || GetExtension(path) == ".jfif"
                || GetExtension(path) == ".webp" || GetExtension(path) == ".pjpeg" || GetExtension(path) == ".avif")
            {
                Console.WriteLine("it's a image with preview");
                return true;
            }
            return false;

        }
        private bool GetVideoState(string path)
        {
            if (GetExtension(path) == ".m4v" || GetExtension(path) == ".mov" || GetExtension(path) == ".mp4")
            {
                Console.WriteLine("it's a video with preview");

                return true;
            }
            return false;

        }

        private void eliminarDuplicadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteDuplicate1();
        }

        private void eliminarFilasVaciasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearEmptyRows();
        }

        private void pegarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                string s = Clipboard.GetText();

                string[] lines = s.Replace("\n", "").Split('\r');

                string[] fields;
                int row = contactsdgv.CurrentCell.RowIndex;
                int col = 0;
                int sum = row + lines.Length;
                int totalrows = contactsdgv.Rows.Cast<DataGridViewRow>().Where(rown => !(rown.Cells[0].Value == null && rown.Cells[1].Value == null)).Count();

                Console.WriteLine(lines.Length);
                Console.WriteLine(row + 2);
                Console.WriteLine(totalrows);


                for (int i = 0; i < sum - totalrows; i++)
                {
                    contactsdgv.Rows.Add();
                }



                foreach (string item in lines)
                {

                    fields = item.Split('\t');
                    foreach (string f in fields)
                    {



                        contactsdgv[col, row].Value = f;



                        col++;



                    }

                    row++;

                    col = 0;
                }

                foreach (DataGridViewRow item in contactsdgv.Rows)
                {
                    item.Cells[2].Value = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Observación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void limpiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contactsdgv.Rows.Clear();
            contactsdgv.Refresh();
        }
    }
    
}