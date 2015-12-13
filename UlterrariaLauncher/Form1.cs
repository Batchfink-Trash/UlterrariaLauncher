using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Diagnostics;

namespace UlterrariaLauncher
{
    public partial class Form1 : Form
    {
        XmlDocument doc = new XmlDocument();
        //         number, link
        Dictionary<string, string> versions = new Dictionary<string, string>();
        int achievementsNo;

        //path to terraria folder
        string path = Properties.Settings.Default.path;

        //list of achievement panels
        List<AchievementPanel> achPnls = new List<AchievementPanel>();

        //timer for achievements.
        private Timer timer;

        public Form1()
        {
            InitializeComponent();

            firstTimeSetUp();

            //                              ---Download versions.xml---
            string xml;
            //get xml from dropbox
            using (WebClient client = new WebClient())
            {
                xml = client.DownloadString("https://www.dropbox.com/s/3pfym0u1kznsqr0/versions.xml?dl=1");
            }

            doc.LoadXml(xml);
            //select nodes to get to version and link
            XmlNodeList nl = doc.SelectNodes("versions");
            XmlNode root = nl[0];

            foreach (XmlNode xnode in root.ChildNodes)
            {
                //add version number to combobox
                versionBox.Items.Add(xnode.ChildNodes.Item(0).InnerText);
                //add version number and link to dictionary
                versions.Add(xnode.ChildNodes.Item(0).InnerText, xnode.ChildNodes.Item(1).InnerText);
            }
            //set combobox to select top version
            versionBox.SelectedIndex = 0;

            addPanels();

            achievementsPnl.HorizontalScroll.Enabled = false;
            achievementsPnl.HorizontalScroll.Visible = false;

            //      --start achievements timer
            InitTimer();
        }

        //EVENT HANDLERS

        //play button
        private void installBtn_Click(object sender, EventArgs e)
        {
            
            //select correct link according to combobox selection
            string link = versions[Convert.ToString(versionBox.SelectedItem)];
            
            try
            {
                //delete zip and extract
                File.Delete(path + @"\Content\Launcher\dl.zip");
                //Array.ForEach(Directory.GetFiles(path + @"\Content\Launcher\extract"), File.Delete);
                string extractPath = path + @"\Content\Launcher\extract";
                foreach (string p in Directory.GetFiles(extractPath))
                {
                    File.Delete(p);
                }
                string[] subFiles = Directory.GetDirectories(extractPath);
                foreach (string dir in subFiles)
                {
                    foreach (string p in Directory.GetFiles(dir))
                    {
                        File.Delete(p);
                    }
                }
            }
            catch (Exception ex)
            {
                progLbl.Text = "ow";
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                progLbl.Text = "Downloading...";
                //download zip from dropbox
                using (var client = new WebClient())
                {
                    //set up event handlers
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);

                    client.DownloadFileAsync(new Uri(link), path + @"\Content\Launcher\dl.zip");
                }
            }
            catch (Exception ex)
            {
                progLbl.Text = "ow";
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //Async download bar yay
            progBar.Maximum = (int)e.TotalBytesToReceive / 100;
            progBar.Value = (int)e.BytesReceived / 100;
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //extract zip to 'extract' folder
            try
            {
                progLbl.Text = "Unzipping...";
                ZipFile.ExtractToDirectory(path + @"\Content\Launcher\dl.zip", path + @"\Content\Launcher\extract");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unzip Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            copyFiles();
        }


        //other methods
        private void copyFiles()
        {
            try
            {
                progLbl.Text = "Copying files...";
                //copy terraria.exe
                File.Move(path + @"\Terraria.exe", path + @"\Terraria_vanilla.exe");
                File.Copy(path + @"\Content\Launcher\extract\Terraria.exe", path + @"\Terraria.exe", true);

                //copy dlls
                File.Copy(path + @"\Content\Launcher\extract\CSteamworks.dll", path + @"\CSteamworks.dll", true);
                File.Copy(path + @"\Content\Launcher\extract\Ionic.Zip.dll", path + @"\Ionic.Zip.dll", true);
                File.Copy(path + @"\Content\Launcher\extract\Newtonsoft.Json.dll", path + @"\Newtonsoft.Json.dll", true);
                File.Copy(path + @"\Content\Launcher\extract\Steamworks.NET.dll", path + @"\Steamworks.NET.dll", true);

                //copy ulterraria content
                //ew visual basic
                new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyDirectory(path + @"\Content\Launcher\extract\Ulterraria", path + @"\Content\Ulterraria", Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs);
                progLbl.Text = "Done!";
                progBar.Value = 0;

                
                
            }
            catch (Exception ex)
            {
                progLbl.Text = "ow";
                MessageBox.Show(ex.Message, "Error1", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void firstTimeSetUp()
        {
            if (!Directory.Exists(path + @"\Content\Launcher"))
            {
                Directory.CreateDirectory(path + @"\Content\Launcher");
                Directory.CreateDirectory(path + @"\Content\Launcher\extract");
                Directory.CreateDirectory(path + @"\Content\Launcher\Watch");
                Directory.CreateDirectory(path + @"\Content\Launcher\img");
            }
        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            if (fdb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = fdb.SelectedPath;
            }

            pathBox.Text = path;
        }

        private void pathBox_TextChanged(object sender, EventArgs e)
        {
            path = pathBox.Text;
            Properties.Settings.Default.path = pathBox.Text;
            Properties.Settings.Default.Save();
        }

        private void playBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string exePath = path + @"\Terraria.exe";
                Process.Start(exePath);
            }
            catch (Exception ex)
            {
                progLbl.Text = "ow";
                MessageBox.Show(ex.Message, "copy Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addPanels()
        {
            //                              ---Download achievements.xml---
            //get xml from dropbox
            string xml;
            XmlNodeList nl = doc.SelectNodes("versions");
            XmlNode root = nl[0];
            using (WebClient client = new WebClient())
            {
                xml = client.DownloadString("https://www.dropbox.com/s/q23km2xt7bevaus/achievements.xml?dl=1");
            }

            doc.LoadXml(xml);
            //select nodes to get to achievements
            nl = doc.SelectNodes("achievements");
            root = nl[0];
            achievementsNo = root.ChildNodes.Count;

            //loop through panels and achievements
            
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                AchievementPanel pnl = new AchievementPanel();
                //get achievement in xml
                XmlNode xnode = root.ChildNodes[i];

                //create achievement panel and controls
                achievementsPnl.Controls.Add(pnl);

                pnl.achieved = Properties.Settings.Default.completedAchieves[i];
                
                //loop through controls in panel
                foreach (Control ctrl in pnl.Controls)
                {
                    //MessageBox.Show(ctrl.Name);
                    //check which control it is eg. title, description, image
                    //title
                    if (ctrl is Label && ctrl.Font.Size == 11)
                    {
                        ctrl.Text = xnode.ChildNodes.Item(0).InnerText;
                    }
                    //description
                    else if (ctrl is Label && ctrl.Font.Size == 8)
                    {
                        if (pnl.achieved) 
                        {
                            //set text
                            ctrl.Text = xnode.ChildNodes.Item(1).InnerText; 
                        }
                        else
                        {
                            ctrl.Text = "?";
                        }                        
                    }
                    else if (ctrl is PictureBox)
                    {
                        if (pnl.achieved)
                        {
                            try
                            {
                                (ctrl as PictureBox).Load(path + @"\Content\Launcher\img" + i + ".jpg");
                            }
                            catch (Exception)
                            {
                                using (WebClient client = new WebClient())
                                {
                                    client.DownloadFile(@"http://www.batchfink.webege.com/img/" + i + ".jpg", path + @"\Content\Launcher\img\" + i + ".jpg");
                                }
                                (ctrl as PictureBox).Load(path + @"\Content\Launcher\img\" + i + ".jpg");
                            }
                        }
                        else
                        {
                            try
                            {
                                (ctrl as PictureBox).Load(path + @"\Content\Launcher\img\idk.jpg");
                            }
                            catch (Exception)
                            {
                                using (WebClient client = new WebClient())
                                {
                                    client.DownloadFile(@"http://www.batchfink.webege.com/img/idk.jpg", path + @"\Content\Launcher\img\idk.jpg");
                                }
                                (ctrl as PictureBox).Load(path + @"\Content\Launcher\img\idk.jpg");
                            }
                        }
                    }
                }

                //Add panel to panel list
                achPnls.Add(pnl);
            }
        }

        private void refreshAchieves()
        {
            //remove previous achievements
            foreach (Control c in achPnls)
            {
                //MessageBox.Show(c.Controls[0].Text);
                achievementsPnl.Controls.Remove(c);
            }
            achPnls.Clear();
            addPanels();
        }

        public void InitTimer()
        {
            timer = new Timer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = 5000;
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e)
        {
            checkWatch();
        }

        private void checkWatch()
        {
            try
            {
                if (Directory.EnumerateFileSystemEntries(path + @"\Content\Launcher\Watch").Any())
                {
                    string[] files = Directory.GetFiles(path + @"\Content\Launcher\Watch");
                    string e = files[0];
                    int achieveCode = Convert.ToInt32(Path.GetFileNameWithoutExtension(e));
                    if (Properties.Settings.Default.completedAchieves[achieveCode])
                    {
                        File.Delete(e);
                    }
                    else
                    {
                        Properties.Settings.Default.completedAchieves[achieveCode] = true;
                        Properties.Settings.Default.Save();
                        refreshAchieves();
                        File.Delete(e);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //revert back to vanilla
        private void revertBtn_Click(object sender, EventArgs e)
        {
            try
            {
                progLbl.Text = "Replacing Terraria.exe...";
                //delete terraria.exe
                File.Delete(path + @"\Terraria.exe");
                //rename backup to Terraria.exe
                File.Move(path + @"\Terraria_vanilla.exe", path + @"\Terraria.exe");
                // :D
                progLbl.Text = "Done!";
            }
            catch
            {
                MessageBox.Show("You've not installed Ulterraria yet.");
            }
        }
    }
}
