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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace M_Centres_2._0
{
    /// <summary>
    /// Interaction logic for report.xaml
    /// </summary>
    public partial class report : Page
    {
        object Previous;
        byte reh = 0;
        bool hidden = false;
     public   byte Reason { get {
                return reh;
            } set {
                reh = value;
                if (reh == 0)
                {
                    opts.Visibility = Visibility.Collapsed;
                    hidden = true;
                    
                }else if (hidden)
                {
                    hidden = false;
                    opts.Visibility = Visibility.Visible;

                }

            }
        }
        public report(object previous,byte reason)
        {
            Previous = previous;
            InitializeComponent();
            Reason = reason;

            txt_Manager.initialize(ref log_box);
          
        }
        
     void   Append(string text)
        {
            Run run = new Run(text);
          
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var i = this.Parent as Window;
           
            i.Content = Previous;
        }
        Process p;
      async  void dllrepair()
        {
            if (opt1.IsChecked == false && opt2.IsChecked == false)
            {
                i++;
           await     txt_Manager.AddErrorHeading("Select an Option first", 6);
              
                oper.IsEnabled = true;
                oper.Content = "Start Operation";
                bck.IsEnabled = true;

                return;
            }
            opt1.IsEnabled = false;
            opt2.IsEnabled = false;
            if (opt1.IsChecked==true)
            {
                specialrestore();

                return;
            }
            DllRepair();
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var g = (sender as Button);
            g.IsEnabled = false;
            g.Content = "Wait A Few Moments";
            bck.IsEnabled = false;
            switch (Reason)
            {
                case 0:
                  await  Dll_method_activate();
                    break;
                case 1:
                    {
                        sive = false;

                        waskilled = false;
                        g.IsEnabled = true;
                        g.Content = "Cancel Operation";
                        dllrepair();
                        break;
                    }
                case 2:
                    {
                        g.IsEnabled = true;
                        g.Content = "Start Operation";
                        bck.IsEnabled = true;
                        waskilled = true;
                        opt1.IsEnabled = true;
                        opt2.IsEnabled = true;
                        p.Kill();
                        Reason = 1;
                        break;
                    }
                    
            }
        }

        private void log_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as RichTextBox).ScrollToEnd();
        }
    }
}
