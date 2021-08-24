using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace M_Centres_2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TrialManage.initialize();
            if (Environment.Is64BitOperatingSystem && (!Environment.Is64BitProcess))
            {
                this.IsEnabled = false;
                MessageBox.Show("Please use x64 version of M Centers");
            }
            if (!Environment.Is64BitProcess)
            {
                nondll_main.IsEnabled = false;
                nondll_main.Content = "Use hack without Install\n(x64 bit users only)";
            }
            if (finder("installed.rmc.j"))
            {
               mian_but.Content = "Uninstall Mode";
            }
            else
                mian_but.Content = "Install Mode";
        }
     bool finder(string path)
        {
            path = "C:\\ProgramData\\m-centre_DLL\\" + path;
            if (File.Exists(path))
                return true;
            return false;
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            conte.Height = (e.NewSize.Height / 2)-100;
            conte.Width = e.NewSize.Width - 45;
            agree.Height = e.NewSize.Height/2;
            agree.Width = e.NewSize.Width -20;
        }

        private void agree_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void dis_push(object sender, MouseEventArgs e)
        {
            if (!d_r)
            {
                var i = (Run)sender;
                i.TextDecorations = TextDecorations.Underline;
                i.FontSize = 23;
                i.Text = "Open Discord!";
            }
        }

        private void dis_pop(object sender, MouseEventArgs e)
        {
            if (!d_r)
            {
                var i = (Run)sender;
                i.TextDecorations = null;
                i.FontSize = 20;
                i.Text = "discord";
            }
        }

        private void yt_push(object sender, MouseEventArgs e)
        {
            if (!d_r)
            {
                var i = (Run)sender;
                i.TextDecorations = TextDecorations.Underline;
                i.FontSize = 23;
                i.Text = "Open Video!";
            }
        }

        private void yt_pop(object sender, MouseEventArgs e)
        {
            if (!d_r)
            {
                var i = (Run)sender;
                i.TextDecorations = null;
                i.FontSize = 20;
                i.Text = "video";
            }
        }
        Timer timer;
        bool started = false;
        byte discord_cooldown = 0;
        byte yt_cooldown = 0;
        byte endtime = 0;
        bool die_started = false;
      async  Task start() {
            if (timer == null)
                timer = new Timer(500);
          
            if(die_started)
                timer.Elapsed -= Timer_Elapsed1;
           
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            endtime = 30;
            started = true;
            die_started = false;
            timer.Start();

        }
        void  stop() {
            timer.Elapsed -= Timer_Elapsed;
            timer.Elapsed += Timer_Elapsed1;
        }

        private void Timer_Elapsed1(object sender, ElapsedEventArgs e)
        {

            if (endtime > 0)
                endtime--;
            if (endtime == 0)
                die_out();
        }

        private void die_out()
        {
            if(timer!=null)
            timer.Dispose();
            timer = null;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (discord_cooldown > 0)
                discord_cooldown -= 1;
            if (yt_cooldown > 0)
                yt_cooldown -= 1;
            if (discord_cooldown == 0 && yt_cooldown == 0)
            {
                started = false;
                die_started = true;
                stop();
            }
                
        }
    bool    d_r=false;
        private async void Run_MouseDown(object sender, MouseButtonEventArgs e)
        {

            d_r = true;
            var i = (Run)sender;
            if (discord_cooldown == 0)
            {
                i.Text = "Opening!";
                Process.Start("https://discord.gg/qnfhEmyDdx");
            }
            if (discord_cooldown > 0)
                i.Text = "Do not spam ;)";

            agree.UpdateLayout();
            if (!started)
            {
                discord_cooldown = 10;
                await start();

            }
            if (started & yt_cooldown > 0 && discord_cooldown == 0)
                discord_cooldown = 10;
            await Task.Delay(1500);
            d_r = false;
            i.Text = "discord";
            agree.UpdateLayout();

        }




        private async void Run_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var i = (Run)sender;
            await Task.Delay(1500);
            d_r = false;
            i.Text = "discord";
            agree.UpdateLayout();

        }

        private async void Run_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            d_r = true;
            var i = (Run)sender;
            if (yt_cooldown == 0)
            {
                i.Text = "Opening!";
                Process.Start("https://youtu.be/STBnlQFv1yg");
            }
            if (yt_cooldown > 0)
                i.Text = "Do not spam ;)";

            agree.UpdateLayout();
            if (!started)
            {
                yt_cooldown = 10;
                await start();

            }
            if (started & discord_cooldown > 0 && yt_cooldown == 0)
                yt_cooldown = 10;
            await Task.Delay(1500);
            d_r = false;
            i.Text = "video";
            agree.UpdateLayout();
        }

        private async  void Run_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            var i = (Run)sender;
            await Task.Delay(1500);
            d_r = false;
            i.Text = "video";
            agree.UpdateLayout();

        }
        report rep;
        non_dll_method non_dll;
        private void mian_but_Click(object sender, RoutedEventArgs e)
        {
            if (rep == null)
            {
                rep = new report(olk,0);

                rep.Unloaded += Rep_Unloaded;
            }
            rep.Reason = 0;
            win.Content = rep;
        }

        private void Rep_Unloaded(object sender, RoutedEventArgs e)
        {
            var i = sender as report;
            i = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (rep == null)
            {
                rep = new report(olk, 1);

                rep.Unloaded += Rep_Unloaded;
            }
            rep.Reason = 1;
            win.Content = rep;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Process.Start("https://discord.gg/qnfhEmyDdx");
        }

        private void button_Click_2(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.youtube.com/c/MCentres99");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (non_dll == null)
            {
                non_dll = new non_dll_method(olk);

                non_dll.Unloaded += Non_dll_Unloaded; ;
            }
           
            win.Content = non_dll;
        }

        private void Non_dll_Unloaded(object sender, RoutedEventArgs e)
        {
            var i = sender as non_dll_method;
            i = null;
        }
    }
}
