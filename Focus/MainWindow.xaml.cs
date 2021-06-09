using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            if(Properties.Settings.Default.SaveFolder.Trim() == "")
            {
                Properties.Settings.Default.SaveFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                Properties.Settings.Default.Save();
            }
        }

        private void Top_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        public async void TurnOff(int WMod = 5)
        {
            if (CMod == Modes.Night && WMod != Modes.Night)
            {
                Storyboard sb = Night.FindResource("Activate") as Storyboard;
                sb.Stop();
            }else if(CMod == Modes.Game && WMod != Modes.Game)
            {
                Storyboard sb = Gaming.FindResource("Activate") as Storyboard;
                sb.Stop();
            }
            else if (CMod == Modes.Work && WMod != Modes.Work)
            {
                Storyboard sb = Working.FindResource("Activate") as Storyboard;
                sb.Stop();
            }

            ChangeMode(Modes.Default);
        }

        private void Night_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                if(CMod == Modes.Night)
                {
                    TurnOff();
                    CMod = Modes.Default;
                }
                else
                {
                    TurnOff(Modes.Night);
                    CMod = Modes.Night;
                    Storyboard sb = Night.FindResource("Activate") as Storyboard;
                    sb.Begin();
                    ChangeMode(Modes.Night);
                }
            }
        }

        private void Gaming_MouseDown(object sender, MouseButtonEventArgs e)
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
                    CMod = Modes.Game;
                    Storyboard sb = Gaming.FindResource("Activate") as Storyboard;
                    sb.Begin();
                    ChangeMode(Modes.Game);
                }
            }
        }

        private void Working_MouseDown(object sender, MouseButtonEventArgs e)
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
                    CMod = Modes.Work;
                    Storyboard sb = Working.FindResource("Activate") as Storyboard;
                    sb.Begin();
                    ChangeMode(Modes.Work);
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
        #endregion
        #region separated desktop
        public void SDesktop(string dir, string wm, string Desktop)
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
                if (!IsDirectoryEmpty(dir + @"\focus\Desktop"))
                {
                    Directory.Delete(dir + @"\focus\Desktop", true);
                }
                DirectoryCopy(Desktop, dir + @"\focus\Desktop");
            }
            //clear desktop
            Debug.WriteLine("Clearing the desktop");
            try
            {
                DirectoryDelete(Desktop);
            }
            catch
            {
                MessageBox.Show("Error while clearing desktop!");
            }
            Debug.WriteLine("Restoring " + wm + " mode desktop");
            if (!IsDirectoryEmpty(dir + @"\focus" + wm + @"\Desktop"))
            {
                DirectoryCopy(dir + @"\focus" + wm + @"\Desktop", Desktop, true);
            }
        }
        #endregion
        public async void ChangeMode(int mode)
        {
            String dir = Properties.Settings.Default.SaveFolder;
            String Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Debug.WriteLine(dir);
            String wm = "";
            if (CMod == Modes.Night)
            {
                wm = @"\Night";
            }
            else if (CMod == Modes.Game)
            {
                wm = @"\Game";
            }
            else if (CMod == Modes.Work)
            {
                wm = @"\Work";
            }
            if (mode == Modes.Night)
            {
                //Separated desktop
                if (Properties.Settings.Default.MDesktop)
                {
                    SDesktop(dir, wm, Desktop);
                }
                #region background
                #endregion

            }else if(mode == Modes.Game)
            {
                if (Properties.Settings.Default.GDesktop)
                {
                    SDesktop(dir, wm, Desktop);
                }
                #region background
                #endregion
            }
            else if (mode == Modes.Work)
            {
                if (Properties.Settings.Default.WDesktop)
                {
                    SDesktop(dir, wm, Desktop);
                }
                #region background
                #endregion
            }
            else if(mode == Modes.Default)
            {
                #region BUP DESKTOP
                String n = "";
                if (CMod == Modes.Night)
                {
                    n = @"\Night";
                }else if(CMod == Modes.Game)
                {
                    n = @"\Game";
                }
                else if(CMod == Modes.Work)
                {
                    n = @"\Work";
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
                }else Directory.Delete(dir + @"\focus" + n + @"\Desktop", true);
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
        //CBoxes
        private void MMCbox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.MDesktop = true;
            SaveP();
        }

        private void MMCbox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.MDesktop = false;
            SaveP();
        }

        private void GMCbox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.GDesktop = true;
            SaveP();
        }

        private void GMCbox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.GDesktop = false;
            SaveP();
        }

        private void WMCbox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.WDesktop = true;
            SaveP();
        }

        private void WMCbox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.WDesktop = false;
            SaveP();
        }
    }
}
