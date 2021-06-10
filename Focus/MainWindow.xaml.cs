using Focus.ForDesktop;
using Focus.timing;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Focus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int CMod = Modes.Default;

        private readonly Desktop _desktop = new Desktop();
        private readonly DesktopRegistry _registry = new DesktopRegistry();
        private readonly Storage _storage = new Storage();


        public IDictionary<string, string> registryValues;
        NamedDesktopPoint[] iconPositions;

        Timingset.TimingDataTable timingTable = new Timingset.TimingDataTable();

        public MainWindow()
        {
            InitializeComponent();
            if (App.PriorProcess() != null)
            {
                //MessageBox.Show("Another instance of the app is already running.");
                System.Windows.Application.Current.Shutdown();
                return;
            }

            if (!Directory.Exists(Paths.Focus))
            {
                Directory.CreateDirectory(Paths.Focus);
            }

            #region Settings
            if (Properties.Settings.Default.SaveFolder.Trim() == "")
            {
                Properties.Settings.Default.SaveFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                Properties.Settings.Default.Save();
            }
            if(Properties.Settings.Default.MBg.Trim() == "" || !File.Exists(Properties.Settings.Default.MBg))
            {
                Properties.Settings.Default.MBg = CurrentWallpapperPath();
                Properties.Settings.Default.Save();
            }
            if (Properties.Settings.Default.GBg.Trim() == "" || !File.Exists(Properties.Settings.Default.GBg))
            {
                Properties.Settings.Default.GBg = CurrentWallpapperPath();
                Properties.Settings.Default.Save();
            }
            if (Properties.Settings.Default.WBg.Trim() == "" || !File.Exists(Properties.Settings.Default.WBg))
            {
                Properties.Settings.Default.WBg = CurrentWallpapperPath();
                Properties.Settings.Default.Save();
            }

            if(Properties.Settings.Default.DBg.Trim() == "" || !File.Exists(Properties.Settings.Default.DBg))
            {
                Properties.Settings.Default.DBg = CurrentWallpapperPath();
                Properties.Settings.Default.Save();
            }
            #endregion
            //SAVE ICON POSITIONS
            registryValues = _registry.GetRegistryValues();
            var iconPositions = _desktop.GetIconsPositions();
            _storage.SaveIconPositions(iconPositions, registryValues, "Default");

            #region DataSet

            if (File.Exists(Paths.XML))
            {
                timingTable.ReadXml(Paths.XML);
            }
            else
            {
                Timingset.TimingRow timingRow = timingTable.NewTimingRow();
                timingRow["StartTime"] = DateTime.Now;
                timingRow["EndTime"] = DateTime.Now.AddDays(1);
                timingRow["Mode"] = 0;
                timingRow["Enabled"] = false;
                timingTable.Rows.Add(timingRow);
                timingTable.WriteXml(Paths.XML);
            }
            CheckTiming();
            #endregion
        }

        private void CheckTiming()
        {
            int mode = Timing.Check();
            if (mode != Modes.Default)
            {
                if (mode == Modes.Night)
                {
                    TurnOff(Modes.Night);
                    Storyboard sb = Night.FindResource("Activate") as Storyboard;
                    sb.Begin();
                    ChangeMode(Modes.Night);
                    CMod = Modes.Night;
                }
                else if (mode == Modes.Game)
                {
                    TurnOff(Modes.Game);
                    Storyboard sb = Gaming.FindResource("Activate") as Storyboard;
                    sb.Begin();
                    ChangeMode(Modes.Game);
                    CMod = Modes.Game;
                }
                else if (mode == Modes.Work)
                {
                    TurnOff(Modes.Work);
                    Storyboard sb = Working.FindResource("Activate") as Storyboard;
                    sb.Begin();
                    ChangeMode(Modes.Work);
                    CMod = Modes.Work;
                }
                else
                {
                    MessageBox.Show("Invalid mode!");
                }
            }
        }

        public void restorePos(string FileName = "Default")
        {
            var rvals = _storage.GetRegistryValues(FileName);
            Debug.WriteLine(rvals.Count);
            _registry.SetRegistryValues(rvals);
            var ipos = _storage.GetIconPositions(FileName);
            _desktop.SetIconPositions(ipos);
        }

        private void Top_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        public async void TurnOff(int WMod = 5)
        {
            if (CMod == Modes.Night && WMod != Modes.Night)
            {
                Storyboard sb = Night.FindResource("Activate") as Storyboard;
                sb.Stop();
            }
            else if (CMod == Modes.Game && WMod != Modes.Game)
            {
                Storyboard sb = Gaming.FindResource("Activate") as Storyboard;
                sb.Stop();
            }
            else if (CMod == Modes.Work && WMod != Modes.Work)
            {
                Storyboard sb = Working.FindResource("Activate") as Storyboard;
                sb.Stop();
            }
            if (WMod == 5)
            {
                ChangeMode(Modes.Default);
            }
        }

        private async void Night_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (CMod == Modes.Night)
                {
                    TurnOff();
                    CMod = Modes.Default;
                }
                else
                {
                    TurnOff(Modes.Night);
                    Storyboard sb = Night.FindResource("Activate") as Storyboard;
                    sb.Begin();
                    ChangeMode(Modes.Night);
                    CMod = Modes.Night;
                }
            }
        }

        private async void Gaming_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (CMod == Modes.Game)
                {
                    TurnOff();
                    CMod = Modes.Default;
                }
                else
                {
                    TurnOff(Modes.Game);
                    Storyboard sb = Gaming.FindResource("Activate") as Storyboard;
                    sb.Begin();
                    ChangeMode(Modes.Game);
                    CMod = Modes.Game;
                }
            }
        }

        private async void Working_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (CMod == Modes.Work)
                {
                    TurnOff();
                    CMod = Modes.Default;
                }
                else
                {
                    TurnOff(Modes.Work);
                    Storyboard sb = Working.FindResource("Activate") as Storyboard;
                    sb.Begin();
                    ChangeMode(Modes.Work);
                    CMod = Modes.Work;
                }
            }
        }

        #region functions

        static byte[] SliceMe(byte[] source, int pos)
        {
            byte[] destfoo = new byte[source.Length - pos];
            Array.Copy(source, pos, destfoo, 0, destfoo.Length);
            return destfoo;
        }

        public static string CurrentWallpapperPath()
        {
            byte[] path = (byte[])Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop").GetValue("TranscodedImageCache");
            return Encoding.Unicode.GetString(SliceMe(path, 24)).TrimEnd("\0".ToCharArray());
        }

        public static void DirectoryDelete(string sourceDirName, bool DelSubDirs = true)
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
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files) file.Delete();
            if (DelSubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    Directory.Delete(subdir.FullName, true);
                }
            }
        }

        public void MKDir(string mdir)
        {
            if (!Directory.Exists(mdir + @"\focus"))
            {
                Directory.CreateDirectory(mdir + @"\focus");
            }
        }

        public bool IsDirectoryEmpty(string path)
        {
            if (!Directory.Exists(path))
            {
                return true;
            }
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs = true)
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
                string tempPath = System.IO.Path.Combine(destDirName, file.Name);
                try
                {
                    file.CopyTo(tempPath, false);
                }
                catch
                {
                }

            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(
        UInt32 action, UInt32 uParam, String vParam, UInt32 winIni);

        private static readonly UInt32 SPI_SETDESKWALLPAPER = 0x14;
        private static readonly UInt32 SPIF_UPDATEINIFILE = 0x01;
        private static readonly UInt32 SPIF_SENDWININICHANGE = 0x02;

        public static void SetWallpapper(string path)
        {
            if (File.Exists(path))
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            else Debug.WriteLine("WALLPAPPER FILE NOT FOUND!!");
        }

        #endregion

        #region separated desktop
        public async void SDesktop(string dir, string wm, string Desktop, bool bdesktop)
        {
            MKDir(dir);
            if (!Directory.Exists(dir + @"\focus" + wm))
            {
                Directory.CreateDirectory(dir + @"\focus" + wm);
            }
            if (!Directory.Exists(dir + @"\focus" + wm + @"\Desktop"))
            {
                Directory.CreateDirectory(dir + @"\focus" + wm + @"\Desktop");
            }

            //Backing up current desktop
            if (!IsDirectoryEmpty(Desktop))
            {
                if(CMod == Modes.Default)
                {
                    if (!IsDirectoryEmpty(dir + @"\focus\Desktop"))
                    {
                        Directory.Delete(dir + @"\focus\Desktop", true);
                    }
                    DirectoryCopy(Desktop, dir + @"\focus\Desktop");

                    //SAVE ICON POSITIONS
                    registryValues = _registry.GetRegistryValues();
                    iconPositions = _desktop.GetIconsPositions();
                    _storage.SaveIconPositions(iconPositions, registryValues, "Default");
                }
                else
                {
                    string n = "";
                    if (CMod == Modes.Night)
                    {
                        n = @"\Night";
                        if (Properties.Settings.Default.MBackground) Properties.Settings.Default.MBg = CurrentWallpapperPath();

                        DesktopRegistry dreg = new DesktopRegistry();
                        var rval = dreg.GetRegistryValues();
                        Desktop d = new Desktop();
                        var ipos = d.GetIconsPositions();
                        _storage.SaveIconPositions(ipos, rval, "Night");
                    }
                    else if (CMod == Modes.Game)
                    {
                        n = @"\Game";
                        if (Properties.Settings.Default.GBackground)
                            Properties.Settings.Default.GBg = CurrentWallpapperPath();

                        DesktopRegistry dreg = new DesktopRegistry();
                        var rval = dreg.GetRegistryValues();
                        Desktop d = new Desktop();
                        var ipos = d.GetIconsPositions();
                        _storage.SaveIconPositions(ipos, rval, "Game");
                    }
                    else if (CMod == Modes.Work)
                    {
                        n = @"\Work";
                        if (Properties.Settings.Default.WBackground)
                            Properties.Settings.Default.WBg = CurrentWallpapperPath();

                        DesktopRegistry dreg = new DesktopRegistry();
                        var rval = dreg.GetRegistryValues();
                        Desktop d = new Desktop();
                        var ipos = d.GetIconsPositions();
                        _storage.SaveIconPositions(ipos, rval, "Work");
                    }

                    MKDir(dir);
                    if (!Directory.Exists(dir + @"\focus" + n))
                    {
                        Directory.CreateDirectory(dir + @"\focus" + n);
                    }
                    if (!Directory.Exists(dir + @"\focus" + n + @"\Desktop"))
                    {
                        Directory.CreateDirectory(dir + @"\focus" + n + @"\Desktop");
                    }
                    //BACKUP
                    if (!IsDirectoryEmpty(Desktop))
                    {
                        if (!IsDirectoryEmpty(dir + @"\focus" + n + @"\Desktop"))
                        {
                            Directory.Delete(dir + @"\focus" + n + @"\Desktop", true);
                        }
                        DirectoryCopy(Desktop, dir + @"\focus" + n + @"\Desktop");
                        Debug.WriteLine(Desktop + " - " + dir + @"\focus" + n + @"\Desktop");
                    }
                    else Directory.Delete(dir + @"\focus" + n + @"\Desktop", true);
                }
            }
            //clear desktop
            try
            {
                DirectoryDelete(Desktop);
            }
            catch
            {
                MessageBox.Show("Error while clearing desktop!");
            }
            if (!IsDirectoryEmpty(dir + @"\focus" + wm + @"\Desktop"))
            {
                DirectoryCopy(dir + @"\focus" + wm + @"\Desktop", Desktop, true);
            }
            _desktop.Refresh();
        }
        #endregion

        public async void ChangeMode(int mode)
        {
            Debug.WriteLine(mode);
            String dir = Properties.Settings.Default.SaveFolder;
            String Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //Debug.WriteLine(dir);
            String wm = "";
            if (mode == Modes.Night)
            {
                wm = @"\Night";
            }
            else if (mode == Modes.Game)
            {
                wm = @"\Game";
            }
            else if (mode == Modes.Work)
            {
                wm = @"\Work";
            }
            #region MODES
            if (mode == Modes.Night)
            {
                //Separated desktop
                if (Properties.Settings.Default.MDesktop)
                {
                    if(CMod == Modes.Default) SDesktop(dir, wm, Desktop, true);
                    else SDesktop(dir, wm, Desktop, false);
                    await Task.Delay(250);

                    Storage str = new Storage();
                    DesktopRegistry dreg = new DesktopRegistry();
                    Desktop d = new Desktop();

                    var rvals = str.GetRegistryValues("Night");
                    dreg.SetRegistryValues(rvals);
                    var ipos = str.GetIconPositions("Night");
                    d.SetIconPositions(ipos);
                }
                #region background
                if (Properties.Settings.Default.MBackground)
                {
                    if(CMod == Modes.Default)
                    {
                        Properties.Settings.Default.DBg = CurrentWallpapperPath();
                        Properties.Settings.Default.Save();
                    }
                    if (File.Exists(Properties.Settings.Default.MBg)) SetWallpapper(Properties.Settings.Default.MBg);
                    else MessageBox.Show("Background file not found! Set the file in the settings!");
                }
                #endregion

            }
            else if (mode == Modes.Game)
            {
                if (Properties.Settings.Default.GDesktop)
                {
                    if (CMod == Modes.Default) SDesktop(dir, wm, Desktop, true);
                    else SDesktop(dir, wm, Desktop, false);
                    await Task.Delay(250);

                    

                    Storage str = new Storage();
                    DesktopRegistry dreg = new DesktopRegistry();
                    Desktop d = new Desktop();

                    var rvals = str.GetRegistryValues("Game");
                    Debug.WriteLine(rvals.Count);
                    dreg.SetRegistryValues(rvals);
                    var ipos = str.GetIconPositions("Game");
                    d.SetIconPositions(ipos);
                }
                #region background
                if (Properties.Settings.Default.GBackground)
                {
                    if (CMod == Modes.Default)
                    {
                        Properties.Settings.Default.DBg = CurrentWallpapperPath();
                        Properties.Settings.Default.Save();
                    }
                    if (File.Exists(Properties.Settings.Default.GBg)) SetWallpapper(Properties.Settings.Default.GBg);
                    else MessageBox.Show("Background file not found! Set the file in the settings!");
                }
                #endregion
            }
            else if (mode == Modes.Work)
            {
                if (Properties.Settings.Default.WDesktop)
                {
                    if (CMod == Modes.Default) SDesktop(dir, wm, Desktop, true);
                    else SDesktop(dir, wm, Desktop, false);
                    await Task.Delay(250);
                    

                    Storage str = new Storage();
                    DesktopRegistry dreg = new DesktopRegistry();
                    Desktop d = new Desktop();

                    var rvals = str.GetRegistryValues("Work");
                    Debug.WriteLine(rvals.Count);
                    dreg.SetRegistryValues(rvals);
                    var ipos = str.GetIconPositions("Work");
                    d.SetIconPositions(ipos);

                }
                #region background
                if (Properties.Settings.Default.WBackground)
                {
                    if (CMod == Modes.Default)
                    {
                        Properties.Settings.Default.DBg = CurrentWallpapperPath();
                        Properties.Settings.Default.Save();
                    }
                    if (File.Exists(Properties.Settings.Default.WBg)) SetWallpapper(Properties.Settings.Default.WBg);
                    else MessageBox.Show("Background file not found! Set the file in the settings!");
                }
                #endregion
            }
            #endregion
            else if (mode == Modes.Default)
            {
                #region BUP DESKTOP
                String n = "";
                if (CMod == Modes.Night)
                {
                    n = @"\Night";
                    if (Properties.Settings.Default.MBackground) Properties.Settings.Default.MBg = CurrentWallpapperPath();

                    if (Properties.Settings.Default.MDesktop)
                    {
                        
                    
                        DesktopRegistry dreg = new DesktopRegistry();
                        var rval = dreg.GetRegistryValues();
                        Desktop d = new Desktop();
                        var ipos = d.GetIconsPositions();
                        _storage.SaveIconPositions(ipos, rval, "Night");
                    }
                }
                else if (CMod == Modes.Game)
                {
                    n = @"\Game";
                    if (Properties.Settings.Default.GBackground) Properties.Settings.Default.GBg = CurrentWallpapperPath();

                    if (Properties.Settings.Default.GDesktop)
                    {
                    
                        DesktopRegistry dreg = new DesktopRegistry();
                        var rval = dreg.GetRegistryValues();
                        Desktop d = new Desktop();
                        var ipos = d.GetIconsPositions();
                        _storage.SaveIconPositions(ipos, rval, "Game");
                    }
                }
                else if (CMod == Modes.Work)
                {
                    n = @"\Work";
                    if (Properties.Settings.Default.WBackground) Properties.Settings.Default.WBg = CurrentWallpapperPath();

                    if (Properties.Settings.Default.WDesktop)
                    {
                    
                        Desktop d = new Desktop();
                        DesktopRegistry dreg = new DesktopRegistry();
                        var rval = dreg.GetRegistryValues();
                        var ipos = d.GetIconsPositions();
                        _storage.SaveIconPositions(ipos, rval, "Work");
                    }
                }

                MKDir(dir);
                if (!Directory.Exists(dir + @"\focus" + n))
                {
                    Directory.CreateDirectory(dir + @"\focus" + n);
                }
                if (!Directory.Exists(dir + @"\focus" + n + @"\Desktop"))
                {
                    Directory.CreateDirectory(dir + @"\focus" + n + @"\Desktop");
                }
                //BACKUP
                if (!IsDirectoryEmpty(Desktop))
                {
                    if (!IsDirectoryEmpty(dir + @"\focus" + n + @"\Desktop"))
                    {
                        Directory.Delete(dir + @"\focus" + n + @"\Desktop", true);
                    }
                    DirectoryCopy(Desktop, dir + @"\focus" + n + @"\Desktop");
                    Debug.WriteLine(Desktop + " - " + dir + @"\focus" + n + @"\Desktop");
                }
                else Directory.Delete(dir + @"\focus" + n + @"\Desktop", true);
                #endregion
                //Clear desktop
                try
                {
                    DirectoryDelete(Desktop);
                }
                catch
                {
                    MessageBox.Show("Error while clearing desktop!");
                }
                if (!IsDirectoryEmpty(dir + @"\focus\Desktop")) DirectoryCopy(dir + @"\focus\Desktop", Desktop, true);
                if (File.Exists(Properties.Settings.Default.DBg)) SetWallpapper(Properties.Settings.Default.DBg);

                _desktop.Refresh();
                await Task.Delay(130);
                restorePos();
                _desktop.Refresh();
            }
        }


        #region Enter Settings Animation
        private async void Settings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SettingsV.Visibility = Visibility.Visible;
            Storyboard sb = this.FindResource("ResizeB") as Storyboard;
            sb.Begin();

            await Task.Delay(6);
            OGrid.Visibility = Visibility.Collapsed;
        }

        private async void Back_Click(object sender, RoutedEventArgs e)
        {
            OGrid.Visibility = Visibility.Visible;
            Storyboard sb = this.FindResource("ResizeS") as Storyboard;
            sb.Begin();
            await Task.Delay(6);
            SettingsV.Visibility = Visibility.Collapsed;
        }

        public void SaveP()
        {
            Properties.Settings.Default.Save();
        }
        #endregion
        #region Checkboxes
        private void MMCbox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.MDesktop = true; SaveP();
        }

        private void MMCbox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.MDesktop = false; SaveP();
        }

        private void GMCbox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.GDesktop = true; SaveP();
        }

        private void GMCbox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.GDesktop = false; SaveP();
        }

        private void WMCbox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.WDesktop = true; SaveP();
        }

        private void WMCbox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.WDesktop = false; SaveP();
        }

        private void MBCbox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.MBackground = true; SaveP();
        }

        private void MBCbox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.MBackground = false; SaveP();
        }

        private void GBCbox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.GBackground = true; SaveP();
        }

        private void GBCbox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.GBackground = false; SaveP();
        }

        private void WBbox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.WBackground = true; SaveP();
        }

        private void WBbox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.WBackground = false; SaveP();
        }
        #endregion

        private void SMBFile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            of.Title = "Select night mode background image";

            of.ShowDialog();
            if (!String.IsNullOrEmpty(of.FileName))
            {
                Properties.Settings.Default.MBg = of.FileName;
                SaveP();
                Debug.WriteLine(of.FileName.ToString());
            }
            else Debug.WriteLine("No file // cancel");
        }

        private void SGBFile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            of.Title = "Select game mode background image";

            of.ShowDialog();
            if (!String.IsNullOrEmpty(of.FileName))
            {
                Properties.Settings.Default.GBg = of.FileName;
                SaveP();
                Debug.WriteLine(of.FileName.ToString());
            }
            else Debug.WriteLine("No file // cancel");
        }

        private void SWBFile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            of.Title = "Select work mode background image";

            of.ShowDialog();
            if (!String.IsNullOrEmpty(of.FileName))
            {
                Properties.Settings.Default.WBg = of.FileName;
                SaveP();
                Debug.WriteLine(of.FileName.ToString());
            }
            else Debug.WriteLine("No file // cancel");
        }

        private void MWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            String dir = Properties.Settings.Default.SaveFolder;
            String Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            #region BUP DESKTOP
            String n = "";
            if (CMod == Modes.Night)
            {
                n = @"\Night";
                if (Properties.Settings.Default.MBackground) Properties.Settings.Default.MBg = CurrentWallpapperPath();

                if (Properties.Settings.Default.MDesktop)
                {


                    DesktopRegistry dreg = new DesktopRegistry();
                    var rval = dreg.GetRegistryValues();
                    Desktop d = new Desktop();
                    var ipos = d.GetIconsPositions();
                    _storage.SaveIconPositions(ipos, rval, "Night");
                }
            }
            else if (CMod == Modes.Game)
            {
                n = @"\Game";
                if (Properties.Settings.Default.GBackground) Properties.Settings.Default.GBg = CurrentWallpapperPath();

                if (Properties.Settings.Default.GDesktop)
                {

                    DesktopRegistry dreg = new DesktopRegistry();
                    var rval = dreg.GetRegistryValues();
                    Desktop d = new Desktop();
                    var ipos = d.GetIconsPositions();
                    _storage.SaveIconPositions(ipos, rval, "Game");
                }
            }
            else if (CMod == Modes.Work)
            {
                n = @"\Work";
                if (Properties.Settings.Default.WBackground) Properties.Settings.Default.WBg = CurrentWallpapperPath();

                if (Properties.Settings.Default.WDesktop)
                {

                    Desktop d = new Desktop();
                    DesktopRegistry dreg = new DesktopRegistry();
                    var rval = dreg.GetRegistryValues();
                    var ipos = d.GetIconsPositions();
                    _storage.SaveIconPositions(ipos, rval, "Work");
                }
            }

            MKDir(dir);
            if (!Directory.Exists(dir + @"\focus" + n))
            {
                Directory.CreateDirectory(dir + @"\focus" + n);
            }
            if (!Directory.Exists(dir + @"\focus" + n + @"\Desktop"))
            {
                Directory.CreateDirectory(dir + @"\focus" + n + @"\Desktop");
            }
            //BACKUP
            if (!IsDirectoryEmpty(Desktop))
            {
                if (!IsDirectoryEmpty(dir + @"\focus" + n + @"\Desktop"))
                {
                    Directory.Delete(dir + @"\focus" + n + @"\Desktop", true);
                }
                DirectoryCopy(Desktop, dir + @"\focus" + n + @"\Desktop");
                Debug.WriteLine(Desktop + " - " + dir + @"\focus" + n + @"\Desktop");
            }
            else Directory.Delete(dir + @"\focus" + n + @"\Desktop", true);
            #endregion
            //Clear desktop
            try
            {
                DirectoryDelete(Desktop);
            }
            catch
            {
                MessageBox.Show("Error while clearing desktop!");
            }
            if (!IsDirectoryEmpty(dir + @"\focus\Desktop")) DirectoryCopy(dir + @"\focus\Desktop", Desktop, true);
            if (File.Exists(Properties.Settings.Default.DBg)) SetWallpapper(Properties.Settings.Default.DBg);

            _desktop.Refresh();
            restorePos();
            _desktop.Refresh();
        }
    }
}