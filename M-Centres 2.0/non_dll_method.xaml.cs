using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for non_dll_method.xaml
    /// </summary>
    public partial class non_dll_method : Page
    {
        object Previous { get; set; }
        public non_dll_method(object previous)
        {
            this.Loaded += Non_dll_method_Loaded;
            Previous = previous;
            TrialManage.initialize();
            InitializeComponent();
        }

        private void Non_dll_method_Loaded(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (app_list.SelectedItem == null)
            {
                non_logger.AppendText("\nSelect an App First\n(Press Reload App List to refresh the list)");
                return;
            }
            

            var g = (app_list.SelectedItem as TextBlock).Text.ToArray();
            
            
            string s = "";
            int i = 0;
            while (i<g.Length)
            {
                if (g[i] == ':')
                    break;
                s += g[i];
                i++;

            }
            s = s.Replace(" ", "");
            try
            {
                var z = Process.GetProcessById(int.Parse(s));
                TrialManage.e += TrialManage_e;
                TrialManage.trialcodecompleted += TrialManage_trialcodecompleted;
                var v = RegisterBlock();
                report = inlinedadder(v);

                TrialManage.RemoveTrial(z);
            }
            catch(ArgumentException ze)
            {
                if(ze.Message.Contains("is not running."))
                {
                    non_logger.AppendText("\n" + ze.Message + "\nRefresh the list");
                }
            }
        }

        private void TrialManage_trialcodecompleted(object sender, RoutedEventArgs e)
        {
            TrialManage.e -= TrialManage_e;
            TrialManage.trialcodecompleted -= TrialManage_trialcodecompleted;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var i = this.Parent as Window;

            i.Content = Previous;
            this.Visibility = Visibility.Collapsed;
        }
    string time()
        {
            bool pm = false;
            var tim = DateTime.Now;
            var hour = tim.Hour;
            if (hour > 12)
            {
                hour -= 12;
                pm = true;
            }
            var min = tim.Minute;
            var sec = tim.Second;
            if(pm)
                return hour.ToString()+":"+min.ToString() + ":" + sec.ToString() + " pm";
            return hour.ToString() + ":" + min.ToString() + ":" + sec.ToString() + " am";
        }
        private void load_app_list(object sender, RoutedEventArgs e)
        {
            app_list.SelectedIndex = -1;
            app_list.Items.Clear();
            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.DoWork += Worker_DoWork;
            non_logger.AppendText("\nReloading List " + time());

            worker.RunWorkerAsync();
            app_list.IsEnabled = false;
            app_load.IsEnabled = false;
            app_load.Content = "Please Wait";
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var path1 = "C:\\Windows\\System32\\Windows.ApplicationModel.Store.dll";
            var path2 = "C:\\Windows\\SysWOW64\\Windows.ApplicationModel.Store.dll";
            Process[] p = Process.GetProcesses();
            foreach (var item in p)
            {
                if (item.ProcessName == "RuntimeBroker")
                    continue;
                try
                {
                    if (!item.HasExited)
                    {
                        foreach (var i in item.Modules)
                        {
                            ProcessModule o = (ProcessModule)i;
                            if (o.ModuleName.Contains("Windows.ApplicationModel.Store"))
                            {
                                Dispatcher.Invoke((Action)(async () =>
                                {

                                    app_list.Items.Add(new TextBlock() { Text= item.Id.ToString() + " : " + item.ProcessName });
                                }));
                            }
                        }
                    }
                }
                catch (System.ComponentModel.Win32Exception)
                {
                   
                }
            }

        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatcher.Invoke((Action)(async () => {

                app_list.IsEnabled = true;
                app_load.IsEnabled = true;
                app_load.Content = "Reload App List";
                non_logger.AppendText("\nReloaded");
            }));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //paste here
            if (app_list.SelectedItem == null)
            {
                non_logger.AppendText("\nSelect an App First\n(Press Reload App List to refresh the list)");
                return;
            }


            var g = (app_list.SelectedItem as TextBlock).Text.ToArray();


            string s = "";
            int i = 0;
            while (i < g.Length)
            {
                if (g[i] == ':')
                    break;
                s += g[i];
                i++;

            }
            s = s.Replace(" ", "");
            try
            {
                var z = Process.GetProcessById(int.Parse(s));
                TrialManage.pe += TrialManage_ep;
                TrialManage.purchasecodecompleted += TrialManage_purchasecodecompleted;
                var v = RegisterBlock();
                report = inlinedadder(v);

                TrialManage.RemovePurchase(z);
            }
            catch (ArgumentException ze)
            {
                if (ze.Message.Contains("is not running."))
                {
                    non_logger.AppendText("\n" + ze.Message + "\nRefresh the list");
                }
            }
        }

        private void TrialManage_purchasecodecompleted(object sender, RoutedEventArgs e)
        {
            TrialManage.pe -= TrialManage_ep;
            TrialManage.purchasecodecompleted -= TrialManage_purchasecodecompleted;

        }

        List<int> processedids;
        BackgroundWorker autoTrial;
        bool autotrialCancelmode = false;
        bool cancelled = false;
        string processname = "";
        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (app_list.SelectedItem == null)
            {
                non_logger.AppendText("\nSelect an App First\n(Press Reload App List to refresh the list)");
                return;
            }
            if (autotrialCancelmode)
            {
                autoTrial.CancelAsync();
                autotrialCancelmode = false;
                while (!cancelled)
                    await Task.Delay(100);
                processedids = null;

                autoTrial = null;
                app_list.IsEnabled = true;
                app_load.IsEnabled = true;
                nondl_back.IsEnabled = true;
                app_purchase.IsEnabled = true;
                app_trial.IsEnabled = true;
                (sender as Button).Content = "Automatically Remove trial";
                non_logger.AppendText("\nCancelled Successfully");
                return;
            }
            processname = (app_list.SelectedItem as TextBlock).Text.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries).Last();
            if (processname.StartsWith(" "))
                processname = processname.Substring(1);
            cancelled = false;
            processedids = new List<int>();
            autoTrial = new BackgroundWorker();
            autoTrial.WorkerSupportsCancellation = true;

            autoTrial.DoWork += AutoTrial_DoWork;
            autoTrial.RunWorkerAsync();
            app_list.IsEnabled = false;
            app_load.IsEnabled = false;
            nondl_back.IsEnabled = false;
            app_purchase.IsEnabled = false;
            app_trial.IsEnabled = false;
            (sender as Button).Content = "Stop operation";
            autotrialCancelmode = true;
        }
        Run inlinedadder(Paragraph g)
        {
            return this.Dispatcher.Invoke(()=>{
                var arg = new Run();
                g.Inlines.Add(arg);
                return arg;
            });
        }
        Paragraph RegisterBlock()
        {
        return    this.Dispatcher.Invoke(() => {
                var k = new Paragraph();

                non_logger.Document.Blocks.Add(k);
                return k;

            });
            
        }
        void setRun(Run s,string text)
        {
           this.Dispatcher.Invoke(() => {
                s.Text = text;
                
             
            });
        }
       string getRun(Run s)
        {
         return   this.Dispatcher.Invoke(() => {
             return s.Text;


            });
        }
        Run report;
        private void AutoTrial_DoWork(object sender, DoWorkEventArgs e)
        {
            TrialManage.e += TrialManage_e;
            var v = RegisterBlock();
           
            var g = (sender as BackgroundWorker);
             report = inlinedadder(v);

            var aRun = inlinedadder( v);
            while (!g.CancellationPending)
            {
               
                setRun(aRun,"\nGetting Processes");
                var k = GetProcesses(processname);
                setRun(aRun,"\nGot Processes");
                foreach(var inp in k)
                {
                    if (!inp.HasExited)
                    {
                        TrialManage.RemoveTrial(inp);
                        setRun(report,getRun(report)+"\nRemoved trial from " + inp.Id.ToString());
                        processedids.Add(inp.Id);
                    }
                    else
                        if (processedids.Contains(inp.Id))
                    {
                        processedids.Remove(inp.Id);
                    }
                }

            }
            TrialManage.e -= TrialManage_e;
            cancelled = true;
        }

        private void TrialManage_e(object sender, RoutedEventArgs e)
        {
            var g = sender as string[];
            if(g[0]== "wroteold")
            {
                setRun(report, getRun(report) + "\n Removed old trial " + g[1]);
                return;
            }
            setRun(report, getRun(report) + "\n Removed new trial " + g[1]);
            return;
        }
        private void TrialManage_ep(object sender, RoutedEventArgs e)
        {
            var g = sender as string[];
            if (g[0] == "wroteold")
            {
                setRun(report, getRun(report) + "\n Removed old Purchase " + g[1]);
                return;
            }
            setRun(report, getRun(report) + "\n Removed new Purchase " + g[1]);
            return;
        }
        List<Process> GetProcesses(string name)
        {
            List<Process> output = new List<Process>();
            var arg = Process.GetProcessesByName(name);
            foreach(var item in arg)
            {
                if (!processedids.Contains(item.Id))
                    output.Add(item);
            }
            return output;
        }

    }
}
