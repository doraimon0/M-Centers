using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Security;
using System.Security.Principal;
using System.Security.AccessControl;
using System.IO;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows;

namespace M_Centres_2._0
{
    public partial class report : Page
    {
        string dll64ver;
        async Task download(string s)
        {
          
                await TrialManage.TryVersionDownloadAsync(s);
          
        }

        private async Task Dll_method_activate()
        {
            //permission
            //delete attempt
            //end runtime brokers
            //end processes
            //delete file
            //replace them
            success = false;
            second = false;
            if (File.Exists("C:\\Windows\\System32\\Windows.ApplicationModel.Store.dll"))
            {
                var ver0 = FileVersionInfo.GetVersionInfo("C:\\Windows\\System32\\Windows.ApplicationModel.Store.dll");
                dll64ver = ver0.FileBuildPart.ToString() + "." + ver0.FilePrivatePart.ToString();

                if (!Directory.Exists(dll64ver))
                {
            await         txt_Manager.AddStepHeading("Downloading " + dll64ver, 1);
                    await download(dll64ver);
                    if (!Directory.Exists(dll64ver))
                    {
                        txt_Manager.AddStep("Incompatible version " + dll64ver + " ,Report this at discord", 1, 3, false);
                        return;
                    }
                }
            }
            await take_permissions("C:\\Windows\\System32\\Windows.ApplicationModel.Store.dll");
          

                   }
        async Task Replace(string Path,bool Is64)
        {
            
            Dispatcher.Invoke((Action)(async () => {

                await txt_Manager.AddStepHeading("Mod Installation", 3);
            }));

            if (Is64)
            {
                File.Copy(dll64ver+"/x64/Windows.ApplicationModel.Store.dll", Path);
                Dispatcher.Invoke((Action)( () => {

                    txt_Manager.AddStep("Success", 1, 3, false);
                }));
                return;
            }
            File.Copy(dll64ver+"/x86/Windows.ApplicationModel.Store.dll", Path);
            Dispatcher.Invoke((Action)( () => {

                txt_Manager.AddStep("Success", 1, 3, false);
            }));
         
        }
    async    Task<bool> Delete(string path)
        {
            Dispatcher.Invoke((Action)(async () => {

                await txt_Manager.AddStepHeading("Deletion", 2);
            }));
            try
            {
               
                    Process[] p = Process.GetProcesses();
                    foreach (var item in p)
                    {

                        try
                        {
                            if (!item.HasExited)
                            {
                                foreach (var i in item.Modules)
                                {
                                    ProcessModule o = (ProcessModule)i;
                                    if (o.FileName == path)
                                        item.Kill();
                                }
                            }
                        }
                        catch (System.ComponentModel.Win32Exception)
                        {

                        }
                    }
                
                 ;
                File.Delete(path);
                Dispatcher.Invoke((Action)( () => {

                    txt_Manager.AddStep("Deleted " + path, 1, 2, true);
                }));
                return true;
            }
            catch (UnauthorizedAccessException e)
            {
                string s = e.Source; 
                Dispatcher.Invoke((Action)(async () => {

                    await txt_Manager.AddErrorLine(e.Message, 3);
                }));
            }
            catch(IOException e)
            {
                Dispatcher.Invoke((Action)(async () => {

                    await txt_Manager.AddErrorLine(e.Message, 3);
                }));
            }
            catch (ArgumentException e)
            {
                Dispatcher.Invoke((Action)(async () => {

                    await txt_Manager.AddErrorLine(e.Message, 3);
                }));
            }
            Dispatcher.Invoke((Action)(async () => {

                await txt_Manager.AddErrorLine("Failed to delete " + path, 3);
            }));
            return false;

        }
        private async Task<bool> verify_permission(string path)
        {
          
             var il=File.GetAccessControl(path);
            var s = il.GetOwner(typeof(NTAccount)).Value;
            Dispatcher.Invoke((Action)(async () => {

                await txt_Manager.AddStepHeading("Ownership Verification done", 2);
            txt_Manager.AddStep("Owned By " + s, 1, 2, false);
            }));
            var i = Environment.UserDomainName;
            var va = Environment.UserName;
            if (s == i + "\\" + va)
            {
                Dispatcher.Invoke((Action)( () => {

                    txt_Manager.AddStep("Results are Valid.", 1, 2, true);
                }));
                return true;
            }
            Dispatcher.Invoke((Action)(async () => {

                await txt_Manager.AddErrorLine("Results are Invalid.   Proceeding to Try the Luck", 4);
            }));
            return false;
        }

        private async Task take_permissions(string path)
        {
            i++;
            if (File.Exists(path))
            {
                var Info = new ProcessStartInfo("cmd.exe", @"/k takeown /f " + path + @" && icacls " + path + " /grant "+"\""+Environment.UserName + "\"" + ":F");
                Info.UseShellExecute = false;
                Info.RedirectStandardOutput = true;
                Info.Verb = "runas";
                Process p = new Process();
                p.StartInfo = Info;
                p.EnableRaisingEvents = true;
               
                p.OutputDataReceived += Ol_OutputDataReceived;
                p.ErrorDataReceived += Ol_OutputDataReceived;
               
                p.Start();
                p.BeginOutputReadLine();

                p.Exited += P_Exited;
             
               
             

          

            }
        }
        bool second = false;
        private async void P_Exited(object sender, EventArgs e)
        {

            if (second)
            {
                await verify_permission("C:\\Windows\\SysWOW64\\Windows.ApplicationModel.Store.dll");
                if (await Delete("C:\\Windows\\SysWOW64\\Windows.ApplicationModel.Store.dll"))
                    await Replace("C:\\Windows\\SysWOW64\\Windows.ApplicationModel.Store.dll", false);
                else
                {
                    Dispatcher.Invoke((Action)(async () =>
                    {

                        await txt_Manager.AddErrorHeading("x86 Mod was unable to install", 2);
                    }));
                }
                Dispatcher.Invoke((Action)( () => {
                    oper.IsEnabled = true;
                    oper.Content = "Start Operation";
                    bck.IsEnabled = true;
                }));
                return;
            }
            await verify_permission("C:\\Windows\\System32\\Windows.ApplicationModel.Store.dll");
            if (await Delete("C:\\Windows\\System32\\Windows.ApplicationModel.Store.dll"))
                await Replace("C:\\Windows\\System32\\Windows.ApplicationModel.Store.dll", Environment.Is64BitProcess);
            else { 
                Dispatcher.Invoke((Action)(async () => {

                    await txt_Manager.AddErrorHeading("x64 Mod Failed to Install", 1);
                })); }
            second = true;
            if (Environment.Is64BitProcess)
            {
                Dispatcher.Invoke((Action)(() => {
                    txt_Manager.AddRun(new System.Windows.Documents.Run("\n\t\t\t\tx86 app steps") { FontSize = 32, Foreground = new SolidColorBrush(Colors.Aqua) });
                    txt_Manager.AddParagraph(new System.Windows.Documents.Run[] { new System.Windows.Documents.Run("") });
                }));
                i++;

                await take_permissions("C:\\Windows\\SysWOW64\\Windows.ApplicationModel.Store.dll");

            }
            else
            {
                Dispatcher.Invoke((Action)( () => {
                    oper.IsEnabled = true;
                    oper.Content = "Start Operation";
                    bck.IsEnabled = true;
                }));
            }



        }
        int v = -1;
        int i { get { return v; } set
            {
                while (value > v)
                {
                    Dispatcher.Invoke((Action)( () => {
                        txt_Manager.AddParagraph(new System.Windows.Documents.Run[] { 
                            new System.Windows.Documents.Run("") });
                    }));
                    v++;
                }
            } }
        bool success = false;
        private void Ol_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
           
        }

        private async void Ol_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(e.Data))
                return;
            if (e.Data == Environment.CurrentDirectory)
            {
                (sender as Process).Kill();
                return;

            }
            i++;
            if (e.Data.Contains("Failed processing 0 files"))
            {
                (sender as Process).Kill();
                success = true;
                this.Dispatcher.Invoke((Action)(async () => {
                    if (success)
                    {
                        if (success)
                            await txt_Manager.EditAt(i, new System.Windows.Documents.Run("\nPermission Taking : Success")
                            {
                                FontSize = 30,
                                FontWeight = FontWeights.SemiBold,
                                Foreground = new SolidColorBrush(Colors.GhostWhite),

                            });
                        else
                            await txt_Manager.EditAt(i, new System.Windows.Documents.Run("\nPermission Taking: Failed")
                            {
                                FontSize = 30,

                                FontWeight = FontWeights.SemiBold,

                                Foreground = new SolidColorBrush(Colors.IndianRed)



                            });
                    }
                    i++;
                  
                }));
            }
            if (e.Data == Environment.CurrentDirectory)
                return;
            this.Dispatcher.Invoke( (Action)( ()=>{
                
                 txt_Manager.AddStep(e.Data, 0, 2, false);

            }));
        }
    }
}
