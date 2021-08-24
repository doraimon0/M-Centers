using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.ComponentModel;
namespace M_Centres_2._0
{
    public partial class report : Page
    {
        bool waskilled = false;
        void specialrestore()
        {
            waskilled = false;
            runs = 1;
            specialrepair("C:\\Windows\\System32\\sfc.exe", "C:\\Windows\\System32\\Windows.ApplicationModel.Store.dll");
        }
        void det(object pa)
        {
            string path = (pa as string[]).First();
            string cmd = (pa as string[]).Last();
            p = new Process();

            p.StartInfo = new ProcessStartInfo(path, " /scanfile=" + cmd) { UseShellExecute = false, RedirectStandardOutput = true };
            p.EnableRaisingEvents = true;

            p.OutputDataReceived += P_OutputDataReceived;
            p.Exited += P_Exited1;
            p.Start();
            p.BeginOutputReadLine();
            
        }
        
        async void specialrepair(string path,string cmd)
        {
            if (waskilled)
                return;
             dat = new List<string>();
            i++;
        await    Dispatcher.Invoke(async() => {
            s = new Run() { Foreground = new SolidColorBrush(Colors.GhostWhite) };

            await txt_Manager.AddStepHeading("Restoring\n\t"+cmd, 4);
            txt_Manager.AddStep("It will require significant time and internet connection.So be Patient", 0, 4, false);
            txt_Manager.AddRun(s);
            });
            i++;
            new Thread(det).Start(new string[] { path, cmd });
        }
        int runs = 0;
        async void DllRepair()
        {
            dat = new List<string>();
            i++;
            s = new Run() { Foreground = new SolidColorBrush(Colors.GhostWhite) };
            await txt_Manager.AddStepHeading("Restoring System Files", 4);
             txt_Manager.AddStep("It will require significant time and internet connection.So be Patient", 0, 4, false);
            txt_Manager.AddRun(s);
            var v = new BackgroundWorker();
            v.DoWork += V_DoWork;
            v.RunWorkerAsync();
               
            i++;     }

        private void V_DoWork(object sender, DoWorkEventArgs e)
        {
            p = new Process();

            p.StartInfo = new ProcessStartInfo("C:\\Windows\\System32\\sfc.exe", " /scannow") { UseShellExecute = false, RedirectStandardOutput = true };
            p.EnableRaisingEvents = true;

            p.OutputDataReceived += P_OutputDataReceived;
            p.Exited += P_Exited1;
            p.Start();
            p.BeginOutputReadLine();

            //   while (!p.HasExited)
            // {
            //   P_OutputDataReceived((char)p.StandardOutput.Read());
            //    }
        }

        string msg = "";
        private void P_OutputDataReceived(object sender,DataReceivedEventArgs e)
        {
            string msga = e.Data;
            
            if (msga == null)
                return;
            msga = msga.Replace("\0", "");
            msga = msga.Replace("\v", "\n");
            msga = msga.Replace("\a", "");
            if (string.IsNullOrEmpty(msga))
                return;
            try
            {
                if (msga != null)
                {
                    if (sive) {
                        Dispatcher.Invoke(() => {
                             s.Text = msg + "\n\t" + msga;
                        });
                    }
                    else

                    if (msga.Contains("V"))
                    {
                        sive = true;
                        Dispatcher.Invoke(() => {
                            msg = s.Text;
                            s.Text = msg + "\n\t" + msga;
                        });
                    }
                    else
                    {
                        Dispatcher.Invoke(() => {
                            s.Text +=  "\n\t" + msga;
                        });
                    }
                
                }
                // or collect the data, etc
            }
            catch (Exception ex)
            {
                dat.Add(ex.Message);

                return;
            }
        }

        Run s;
        bool sive = false;
        List<string> dat;
        private void P_Exited1(object sender, EventArgs e)
        {
            sive = false;
            Task.Delay(5000).Wait();
            if (runs == 1)
            {
                runs = 2;
                if (Environment.Is64BitProcess)
                {
                    specialrepair("C:\\Windows\\System32\\sfc.exe", "C:\\Windows\\SysWOW64\\Windows.ApplicationModel.Store.dll");
                    return;
                }
            }
            Dispatcher.Invoke((Action)( () => {
                runs = 0;
                opt1.IsEnabled = true;
                opt2.IsEnabled = true;

                oper.IsEnabled = true;
                oper.Content = "Start Operation";
                bck.IsEnabled = true;
            }));
        }
    }
}
