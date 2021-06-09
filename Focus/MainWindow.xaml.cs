using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
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
        }

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
            if(WMod == 5) ChangeMode(Modes.Default);
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
                    //Debug.WriteLine("Already");
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
        public void SDesktop(string dir, string wm, string Desktop, bool bdesktop)
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
            if (!IsDirectoryEmpty(Desktop) && bdesktop)
            {
                if (!IsDirectoryEmpty(dir + @"\focus\Desktop"))
                {
                    Directory.Delete(dir + @"\focus\Desktop", true);
                }
                DirectoryCopy(Desktop, dir + @"\focus\Desktop");
            }
            //clear desktop
            //Debug.WriteLine("Clearing the desktop");
            try
            {
                DirectoryDelete(Desktop);
            }
            catch
            {
                MessageBox.Show("Error while clearing desktop!");
            }
            //Debug.WriteLine("Restoring " + wm + " mode desktop");
            if (!IsDirectoryEmpty(dir + @"\focus" + wm + @"\Desktop"))
            {
                DirectoryCopy(dir + @"\focus" + wm + @"\Desktop", Desktop, true);
            }
            //Debug.WriteLine("Toggle Desktop icons!");
            ToggleDesktopIcons();
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
            if (mode == Modes.Night)
            {
                //Separated desktop
                if (Properties.Settings.Default.MDesktop)
                {
                    if(CMod == Modes.Default) SDesktop(dir, wm, Desktop, true);
                    else SDesktop(dir, wm, Desktop, false);
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
            else if (mode == Modes.Default)
            {
                #region BUP DESKTOP
                String n = "";
                if (CMod == Modes.Night)
                {
                    n = @"\Night";
                    if (Properties.Settings.Default.MBackground)
                        Properties.Settings.Default.MBg = CurrentWallpapperPath();
                }
                else if (CMod == Modes.Game)
                {
                    n = @"\Game";
                    if (Properties.Settings.Default.GBackground)
                        Properties.Settings.Default.GBg = CurrentWallpapperPath();
                }
                else if (CMod == Modes.Work)
                {
                    n = @"\Work";
                    if (Properties.Settings.Default.WBackground)
                        Properties.Settings.Default.WBg = CurrentWallpapperPath();
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
            }
        }


        //ENTER SETTINGS
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

        #region Refresh desktop
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        private const int WM_COMMAND = 0x111;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);


        public static string GetWindowText(IntPtr hWnd)
        {
            int size = GetWindowTextLength(hWnd);
            if (size++ > 0)
            {
                var builder = new StringBuilder(size);
                GetWindowText(hWnd, builder, builder.Capacity);
                return builder.ToString();
            }

            return String.Empty;
        }

        public static IEnumerable<IntPtr> FindWindowsWithClass(string className)
        {
            IntPtr found = IntPtr.Zero;
            List<IntPtr> windows = new List<IntPtr>();

            EnumWindows(delegate (IntPtr wnd, IntPtr param)
            {
                StringBuilder cl = new StringBuilder(256);
                GetClassName(wnd, cl, cl.Capacity);
                if (cl.ToString() == className && (GetWindowText(wnd) == "" || GetWindowText(wnd) == null))
                {
                    windows.Add(wnd);
                }
                return true;
            },
                        IntPtr.Zero);

            return windows;
        }

        static void ToggleDesktopIcons()
        {
            var toggleDesktopCommand = new IntPtr(0x7402);
            IntPtr hWnd = IntPtr.Zero;
            if (Environment.OSVersion.Version.Major < 6 || Environment.OSVersion.Version.Minor < 2) //7 and -
                hWnd = GetWindow(FindWindow("Progman", "Program Manager"), GetWindow_Cmd.GW_CHILD);
            else
            {
                var ptrs = FindWindowsWithClass("WorkerW");
                int i = 0;
                while (hWnd == IntPtr.Zero && i < ptrs.Count())
                {
                    hWnd = FindWindowEx(ptrs.ElementAt(i), IntPtr.Zero, "SHELLDLL_DefView", null);
                    i++;
                }
            }


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
    }
}