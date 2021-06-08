using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void ChangeMode(int mode)
        {
            String dir = Properties.Settings.Default.SaveFolder;
            Debug.WriteLine(dir);
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
    }
}
