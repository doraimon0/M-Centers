using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace M_Centres_2._0
{
    class codepattern
    {
        public string address { get; set; }
        public string opcode { get; set; }

    }
    class ph
    {
        public codepattern old { get; set; }
        public codepattern now { get; set; }

    }
    class opcodeData
    {
        public ph Purchase { get; set; }
        public ph Trial { get; set; }
    }
 static class TrialManage
    {
        static HttpClient Client;
        static WebClient client;
        static class Paths
        {
            public static string basex64Dll = "https://raw.githubusercontent.com/tinedpakgamer/mcenterdlls/main/{identity}/x64/Windows.ApplicationModel.Store.dll";
            public static string basex64opcode = "https://raw.githubusercontent.com/tinedpakgamer/mcenterdlls/main/{identity}/x64/opcodes.json";
            public static string basex86Dll = "https://raw.githubusercontent.com/tinedpakgamer/mcenterdlls/main/{identity}/x86/Windows.ApplicationModel.Store.dll";
            public static string basex86opcode = "https://raw.githubusercontent.com/tinedpakgamer/mcenterdlls/main/{identity}/x86/opcodes.json";

            public static List<string> database { 
               get  {
                    if (!Directory.Exists("temp"))
                        Directory.CreateDirectory("temp");
                    if (File.Exists("temp/main"))
                    File.Delete("temp/main");
                retry:;
                    try
                    {
                      
                         client.DownloadFile("https://raw.githubusercontent.com/tinedpakgamer/mcenterdlls/main/main", "temp/main");
                    }
                    catch(WebException es)
                    {
                        
                        var a=MessageBox.Show(es.Message + " - " + es.Status.ToString()+"\nRetry?","error occured",MessageBoxButton.YesNo);
                        if (a == MessageBoxResult.Yes || a == MessageBoxResult.OK||a==MessageBoxResult.None)
                        {
                            goto retry;
                        }
                        else
                        {
                            return null;
                        }
                    }
                
                        var s=File.ReadAllText("temp/main");
                    var fg = s;
                    for (int i = 0; i < 10; ++i)
                        fg = fg.Replace(i.ToString(), "");
                    fg = fg.Replace(".", "");
                    fg = fg.Replace(",", "");
                    var vb = fg.ToArray();
                    foreach(var k in vb)
                    {
                        s = s.Replace(k.ToString(), "");
                    }
                    return s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        
                
                } }
        }
        static void TryDownload(string s,string p)
        {retry:;
            try
            {
             client.DownloadFile(s, p);
             

            }
       
            catch (WebException es)
            {
                var a = MessageBox.Show(es.Message+ "\nRetry?", p, MessageBoxButton.YesNo);
                if (a == MessageBoxResult.Yes || a == MessageBoxResult.OK || a == MessageBoxResult.None)
                {
                    goto retry;
                }
                else
                {
                    return ;
                }
            }
        }
        public static void TryVersionDownload(string identity)
        {
            var database = Paths.database;
            if (database == null)
            {
                throw new Exception("Canceled by user");
            }
                if (!database.Contains(identity))
            {
                throw new Exception("Unknown module " + identity + " Report this at discord");
               
            }

            Directory.CreateDirectory(identity + "/x64");
            Directory.CreateDirectory(identity + "/x86");
            var x64dlldownload = Paths.basex64Dll.Replace("{identity}", identity);
            var x64opcodedownload = Paths.basex64opcode.Replace("{identity}", identity);
            var x86dlldownload = Paths.basex86Dll.Replace("{identity}", identity);
            var x86opcodedownload = Paths.basex86opcode.Replace("{identity}", identity);
            TryDownload(x64dlldownload, identity + "/x64/Windows.ApplicationModel.Store.dll");
            TryDownload(x64opcodedownload, identity + "/x64/opcodes.json");
            TryDownload(x86dlldownload, identity + "/x86/Windows.ApplicationModel.Store.dll");
            TryDownload(x86opcodedownload, identity + "/x86/opcodes.json");
        }
        public static async Task TryVersionDownloadAsync(string identity)
        {
            await Task.Run(() => { 
            var database = Paths.database;
            if (database == null)
            {
                throw new Exception("Canceled by user");
            }
            if (!database.Contains(identity))
            {
                throw new Exception("Unknown module " + identity + " Report this at discord");

            }

            Directory.CreateDirectory(identity + "/x64");
            Directory.CreateDirectory(identity + "/x86");
            var x64dlldownload = Paths.basex64Dll.Replace("{identity}", identity);
            var x64opcodedownload = Paths.basex64opcode.Replace("{identity}", identity);
            var x86dlldownload = Paths.basex86Dll.Replace("{identity}", identity);
            var x86opcodedownload = Paths.basex86opcode.Replace("{identity}", identity);
            TryDownload(x64dlldownload, identity + "/x64/Windows.ApplicationModel.Store.dll");
            TryDownload(x64opcodedownload, identity + "/x64/opcodes.json");
            TryDownload(x86dlldownload, identity + "/x86/Windows.ApplicationModel.Store.dll");
            TryDownload(x86opcodedownload, identity + "/x86/opcodes.json");
            });
        }
        public    static void initialize()
        {
            Client = new HttpClient();
            client = new WebClient();
            var a = new WebHeaderCollection();
            a.Add("MyApplication", "1");
            client.Headers = a;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            trialopcode = new byte[] { 0xb1, 0x00, 0x90 };
            purchaseopcode = new byte[] { 0x31, 0xc9, 0x90 };
        }
        static byte[] trialopcode;
        static byte[] purchaseopcode;
        static int Last = -1;
        static ProcessMemoryReader reader;
     public   static event RoutedEventHandler e;
        public static event RoutedEventHandler trialcodecompleted;
        public static event RoutedEventHandler pe;
        public static event RoutedEventHandler purchasecodecompleted;
        static ProcessModule mod;
        public static void trialmodWriter(string sz, long old_offset, long new_offset)
        {
            int i;
            reader.WriteProcessSpecial(old_offset, trialopcode, out i);
            e.Invoke(new string[] { "wroteold", i.ToString() + " " + sz }, null);
            int vs;
            reader.WriteProcessSpecial(new_offset, trialopcode, out vs);
            e.Invoke(new string[] { "wrotenew", vs.ToString() + " " + sz }, null);

        }
        public static void PurchasemodWriter(string sz,  long old_offset,long new_offset)
        {
            int i;
            reader.WriteProcessSpecial(old_offset, purchaseopcode, out i);
            pe.Invoke(new string[] { "wroteold", i.ToString() + " " + sz }, null);
            int vs;
            reader.WriteProcessSpecial(new_offset, purchaseopcode, out vs);
            pe.Invoke(new string[] { "wrotenew", vs.ToString() + " " + sz }, null);

        }


        //h
        public static void trialmodWriter(string sz, long old_offset, long new_offset,byte[] oldcode,byte[] newcode)
        {
            int i;
            reader.WriteProcessSpecial(old_offset, oldcode, out i);
            e.Invoke(new string[] { "wroteold", i.ToString() + " " + sz }, null);
            int vs;
            reader.WriteProcessSpecial(new_offset, newcode, out vs);
            e.Invoke(new string[] { "wrotenew", vs.ToString() + " " + sz }, null);

        }
        public static void PurchasemodWriter(string sz, long old_offset, long new_offset,byte[] oldcode,byte[] newcode)
        {
            int i;
            reader.WriteProcessSpecial(old_offset, oldcode, out i);
            pe.Invoke(new string[] { "wroteold", i.ToString() + " " + sz }, null);
            int vs;
            reader.WriteProcessSpecial(new_offset, newcode, out vs);
            pe.Invoke(new string[] { "wrotenew", vs.ToString() + " " + sz }, null);

        }
        public static void RemovePurchase(Process p)
        {
            
            if (Last == -1 || Last != p.Id)
            {
                reader = null;
                reader = new ProcessMemoryReader();


            rego:;
                try
                {
                    var v = p.Modules;
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    p.Refresh();

                    goto rego;
                }

                while (reader.baseAddress == 0)
                {
                    p.Refresh();
                    foreach (ProcessModule item in p.Modules)
                    {
                        if (item.FileName == "C:\\Windows\\System32\\Windows.ApplicationModel.Store.dll" || item.FileName == "C:\\Windows\\SysWOW64\\Windows.ApplicationModel.Store.dll")
                        {
                            reader.baseAddress = item.BaseAddress.ToInt64();
                            mod = item;
                            break;
                        }
                    }
                }
                reader.ReadProcess = p;
                reader.OpenProcess();
                Last = p.Id;
            }
            if (IsWin64Emulator(p))
            {
                var sz = mod.FileVersionInfo.FileBuildPart.ToString() + "." + mod.FileVersionInfo.FilePrivatePart.ToString();
            reattemptx86:;
                if (Directory.Exists(sz))
                {
                    if (File.Exists(sz + "/x86/opcodes.json"))
                    {
                        var g = File.ReadAllText(sz + "/x86/opcodes.json");
                        var hj=Newtonsoft.Json.JsonConvert.DeserializeObject<opcodeData>(g);
                        if (hj.Purchase.old.opcode == "default"&&hj.Purchase.now.opcode=="default")
                        {
                            try
                            {
                                var av = long.Parse(hj.Purchase.old.address, System.Globalization.NumberStyles.HexNumber);
                                var bv = long.Parse(hj.Purchase.now.address, System.Globalization.NumberStyles.HexNumber);
                                PurchasemodWriter(sz, av,bv);
                            }
                            catch(ArgumentNullException e)
                            {
                                pe.Invoke(new string[] { "", e.ParamName+" - "+e.Message }, null);

                            }
                            catch (EncoderFallbackException e)
                            {
                                pe.Invoke(new string[] { "", e.ParamName + " - " + e.Message }, null);

                            }
                            goto x86purchaseExit;
                        }
                        var ao = hj.Purchase.old.opcode.ToCharArray();
                        byte[] oldop = new byte[ao.Length / 2];
                        int index = 0;
                        for(int i=0;i<oldop.Length;++i) {
                            oldop[i] = Byte.Parse(new string(ao, index, 2));
                            index += 2;
                                }
                        var aonew = hj.Purchase.now.opcode.ToCharArray();
                        byte[] newop = new byte[aonew.Length / 2];
                        int index2 = 0;
                        for (int i = 0; i < newop.Length; ++i)
                        {
                            newop[i] = Byte.Parse(new string(aonew, index2, 2));
                            index2 += 2;
                        }
                        var a = long.Parse(hj.Purchase.old.address, System.Globalization.NumberStyles.HexNumber);
                        var b = long.Parse(hj.Purchase.now.address, System.Globalization.NumberStyles.HexNumber);

                        PurchasemodWriter(sz, a, b, oldop, newop);
                        goto x86purchaseExit;
                    }
                    pe.Invoke(new string[] { "", sz + " x86 opcodes file was not found" }, null);
                    goto x86purchaseExit;
                }
                pe.Invoke(new string[] { "", "downloading " + sz }, null);
                TryVersionDownload(sz);
                if (Directory.Exists(sz))
                    goto reattemptx86;
                    pe.Invoke(new string[] { "", "Unknown module version" + sz + " ,Report this at discord" }, null);

            x86purchaseExit:;
                reader.CloseHandle();
                Last = -1;

                purchasecodecompleted.Invoke(null, null);
                return;
            }

            var s = mod.FileVersionInfo.FileBuildPart.ToString() + "." + mod.FileVersionInfo.FilePrivatePart.ToString();
        reattemptx64:;
            if (Directory.Exists(s))
            {
                if (File.Exists(s + "/x64/opcodes.json"))
                {
                    var g = File.ReadAllText(s + "/x64/opcodes.json");
                    var hj = Newtonsoft.Json.JsonConvert.DeserializeObject<opcodeData>(g);
                    if (hj.Purchase.old.opcode == "default" && hj.Purchase.now.opcode == "default")
                    {
                        try
                        {
                            var av = long.Parse(hj.Purchase.old.address, System.Globalization.NumberStyles.HexNumber);
                            var bv = long.Parse(hj.Purchase.now.address, System.Globalization.NumberStyles.HexNumber);
                            PurchasemodWriter(s, av, bv);
                        }
                        catch (ArgumentNullException e)
                        {
                            pe.Invoke(new string[] { "", e.ParamName + " - " + e.Message }, null);

                        }
                        catch (EncoderFallbackException e)
                        {
                            pe.Invoke(new string[] { "", e.ParamName + " - " + e.Message }, null);

                        }
                        goto x64purchaseExit;
                    }
                    var ao = hj.Purchase.old.opcode.ToCharArray();
                    byte[] oldop = new byte[ao.Length / 2];
                    int index = 0;
                    for (int i = 0; i < oldop.Length; ++i)
                    {
                        oldop[i] = Byte.Parse(new string(ao, index, 2));
                        index += 2;
                    }
                    var aonew = hj.Purchase.now.opcode.ToCharArray();
                    byte[] newop = new byte[aonew.Length / 2];
                    int index2 = 0;
                    for (int i = 0; i < newop.Length; ++i)
                    {
                        newop[i] = Byte.Parse(new string(aonew, index2, 2));
                        index2 += 2;
                    }
                    var a = long.Parse(hj.Purchase.old.address, System.Globalization.NumberStyles.HexNumber);
                    var b = long.Parse(hj.Purchase.now.address, System.Globalization.NumberStyles.HexNumber);
                    
                    PurchasemodWriter(s, a, b, oldop, newop);
                    goto x64purchaseExit;
                }
                pe.Invoke(new string[] { "", s + " x64 opcodes file was not found" }, null);
                goto x64purchaseExit;
            }
            pe.Invoke(new string[] { "", "downloading " + s }, null);

            TryVersionDownload(s);
            if (Directory.Exists(s))
                goto reattemptx64;
            pe.Invoke(new string[] { "", "Unknown module version" + s + " ,Report this at discord" }, null);

        x64purchaseExit:;
           
            purchasecodecompleted.Invoke(null, null);
            reader.CloseHandle();
            Last = -1;

        }

        public static void RemoveTrial(Process p)
        {
            if (Last == -1 || Last != p.Id)
            {
                reader = null;
                reader = new ProcessMemoryReader();
          
                
            rego:;
                try
                {
                    var v = p.Modules;
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    p.Refresh();

                    goto rego;
                }
           
                while (reader.baseAddress == 0)
                {
                    p.Refresh();
                    foreach (ProcessModule item in p.Modules)
                    {
                        if (item.FileName == "C:\\Windows\\System32\\Windows.ApplicationModel.Store.dll" || item.FileName == "C:\\Windows\\SysWOW64\\Windows.ApplicationModel.Store.dll")
                        {
                            reader.baseAddress = item.BaseAddress.ToInt64();
                            mod = item;
                            break;
                        }
                    }
                }
                reader.ReadProcess = p;
                reader.OpenProcess();
                Last = p.Id;
            }
            if (IsWin64Emulator(p))
            {
                var sz = mod.FileVersionInfo.FileBuildPart.ToString() + "." + mod.FileVersionInfo.FilePrivatePart.ToString();
            reattemptx86:;
                if (Directory.Exists(sz))
                {
                    if (File.Exists(sz + "/x86/opcodes.json"))
                    {
                        var g = File.ReadAllText(sz + "/x86/opcodes.json");
                        var hj = Newtonsoft.Json.JsonConvert.DeserializeObject<opcodeData>(g);
                        if (hj.Trial.old.opcode == "default" && hj.Trial.now.opcode == "default")
                        {
                            try
                            {
                                var av = long.Parse(hj.Trial.old.address, System.Globalization.NumberStyles.HexNumber);
                                var bv = long.Parse(hj.Trial.now.address, System.Globalization.NumberStyles.HexNumber);
                                trialmodWriter(sz, av, bv);
                            }
                            catch (ArgumentNullException es)
                            {
                                e.Invoke(new string[] { "", es.ParamName + " - " + es.Message }, null);

                            }
                            catch (EncoderFallbackException es)
                            {
                                e.Invoke(new string[] { "", es.ParamName + " - " + es.Message }, null);

                            }
                            goto x86TrialExit;
                        }
                        var ao = hj.Trial.old.opcode.ToCharArray();
                        byte[] oldop = new byte[ao.Length / 2];
                        int index = 0;
                        for (int i = 0; i < oldop.Length; ++i)
                        {
                            oldop[i] = Byte.Parse(new string(ao, index, 2));
                            index += 2;
                        }
                        var aonew = hj.Trial.now.opcode.ToCharArray();
                        byte[] newop = new byte[aonew.Length / 2];
                        int index2 = 0;
                        for (int i = 0; i < newop.Length; ++i)
                        {
                            newop[i] = Byte.Parse(new string(aonew, index2, 2));
                            index2 += 2;
                        }
                        var a = long.Parse(hj.Trial.old.address, System.Globalization.NumberStyles.HexNumber);
                        var b = long.Parse(hj.Trial.now.address, System.Globalization.NumberStyles.HexNumber);
                        
                        trialmodWriter(sz, a, b, oldop, newop);
                        goto x86TrialExit;
                    }
                    e.Invoke(new string[] { "", sz + " x86 opcodes file was not found" }, null);
                    goto x86TrialExit;
                }
                e.Invoke(new string[] { "", "downloading " + sz }, null);

                TryVersionDownload(sz);
                if (Directory.Exists(sz))
                    goto reattemptx86;
                e.Invoke(new string[] { "", "Unknown module version" + sz + " ,Report this at discord" }, null);

            x86TrialExit:;
                reader.CloseHandle();
                Last = -1;
               
                trialcodecompleted.Invoke(null, null);
                return;
            }

            var s = mod.FileVersionInfo.FileBuildPart.ToString() + "." + mod.FileVersionInfo.FilePrivatePart.ToString();
        reattemptx64:;
            if (Directory.Exists(s))
            {
                if (File.Exists(s + "/x64/opcodes.json"))
                {
                    var g = File.ReadAllText(s + "/x64/opcodes.json");
                    var hj = Newtonsoft.Json.JsonConvert.DeserializeObject<opcodeData>(g);
                    if (hj.Trial.old.opcode == "default" && hj.Trial.now.opcode == "default")
                    {
                        try
                        {
                            var av = long.Parse(hj.Trial.old.address, System.Globalization.NumberStyles.HexNumber);
                            var bv = long.Parse(hj.Trial.now.address, System.Globalization.NumberStyles.HexNumber);
                            trialmodWriter(s, av, bv);
                        }
                        catch (ArgumentNullException es)
                        {
                            e.Invoke(new string[] { "", es.ParamName + " - " + es.Message }, null);

                        }
                        catch (EncoderFallbackException es)
                        {
                            e.Invoke(new string[] { "", es.ParamName + " - " + es.Message }, null);

                        }
                        goto x64TrialExit;
                    }
                    var ao = hj.Trial.old.opcode.ToCharArray();
                    byte[] oldop = new byte[ao.Length / 2];
                    int index = 0;
                    for (int i = 0; i < oldop.Length; ++i)
                    {
                        oldop[i] = Byte.Parse(new string(ao, index, 2));
                        index += 2;
                    }
                    var aonew = hj.Trial.now.opcode.ToCharArray();
                    byte[] newop = new byte[aonew.Length / 2];
                    int index2 = 0;
                    for (int i = 0; i < newop.Length; ++i)
                    {
                        newop[i] = Byte.Parse(new string(aonew, index2, 2));
                        index2 += 2;
                    }
                    var a = long.Parse(hj.Trial.old.address, System.Globalization.NumberStyles.HexNumber);
                    var b = long.Parse(hj.Trial.now.address, System.Globalization.NumberStyles.HexNumber);

                    trialmodWriter(s, a, b, oldop, newop);
                    goto x64TrialExit;
                }
                e.Invoke(new string[] { "", s + " x64 opcodes file was not found" }, null);
                goto x64TrialExit;
            }
            e.Invoke(new string[] { "", "downloading " + s }, null);

            TryVersionDownload(s);
            if (Directory.Exists(s))
                goto reattemptx64;
            e.Invoke(new string[] { "", "Unknown module version" + s + " ,Report this at discord" }, null);

        x64TrialExit:;

            trialcodecompleted.Invoke(null, null);
            reader.CloseHandle();
            Last = -1;

        }

        private static bool IsWin64Emulator(this Process process)
        {
            if ((Environment.OSVersion.Version.Major > 5)
                || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1)))
            {
                bool retVal;

                return NativeMethods.IsWow64Process(process.Handle, out retVal) && retVal;
            }

            return false; // not on 64-bit Windows Emulator
        }
    

    internal static class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);
    }
}
    class ProcessMemoryReader
    {

        public ProcessMemoryReader()
        {
        }

        /// <summary>	
        /// Process from which to read		
        /// </summary>
        public Process ReadProcess
        {
            get
            {
                return m_ReadProcess;
            }
            set
            {
                m_ReadProcess = value;
            }
        }
        public int bise = 0;
        public long baseAddress = 0;

        private Process m_ReadProcess = null;

        private IntPtr m_hProcess = IntPtr.Zero;
        public byte[] jump(long origin, long destination)
        {

            if (destination > origin)
            {
                var j = (int)(destination - origin);
                var i = BitConverter.GetBytes(j).ToList();

                return i.ToArray();
            }
            var iA = BitConverter.GetBytes((int)(origin - destination)).ToList();
            bool added = false;
            for (int v = 0; v < iA.Count; ++v)
            {
                iA[v] = (byte)(255 - iA[v]);
                if (!added)
                {
                    if (iA[v] != 255)
                    {
                        iA[v] += 1;
                        added = true;
                    }
                    else
                        iA[v] = 0;
                }
            }
            return iA.ToArray();



        }
        public void OpenProcess()
        {
            //			m_hProcess = ProcessMemoryReaderApi.OpenProcess(ProcessMemoryReaderApi.PROCESS_VM_READ, 1, (uint)m_ReadProcess.Id);
            ProcessMemoryReaderApi.ProcessAccessType access;
            access = ProcessMemoryReaderApi.ProcessAccessType.PROCESS_VM_READ
                | ProcessMemoryReaderApi.ProcessAccessType.PROCESS_VM_WRITE
                | ProcessMemoryReaderApi.ProcessAccessType.PROCESS_VM_OPERATION;
            m_hProcess = ProcessMemoryReaderApi.OpenProcess((uint)access, 1, (uint)m_ReadProcess.Id);
          
        }

        public void CloseHandle()
        {
            try
            {
                int iRetValue;
                iRetValue = ProcessMemoryReaderApi.CloseHandle(m_hProcess);
                if (iRetValue == 0)
                {
                    throw new Exception("CloseHandle failed");
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }
        public IntPtr CreateMemory(int size)
        {
            IntPtr f = (IntPtr)(baseAddress + 0xDE3A38);
            var s = ProcessMemoryReaderApi.VirtualAllocEx(m_hProcess, out f, (uint)size, (uint)(ProcessMemoryReaderApi.flAllocationType.MEM_COMMIT | ProcessMemoryReaderApi.flAllocationType.MEM_RESERVE), (uint)ProcessMemoryReaderApi.flProtect.PAGE_EXECUTE_READWRITE);
            return f;

        }
        public void WriteProcessSpecial(long offset, byte[] bytesToWrite, out int bytesWritten)
        {
            IntPtr MemoryAddress = (IntPtr)(baseAddress + offset);
            IntPtr ptrBytesWritten;
            ProcessMemoryReaderApi.WriteProcessMemory(m_hProcess, MemoryAddress, bytesToWrite, (uint)bytesToWrite.Length, out ptrBytesWritten);

            bytesWritten = ptrBytesWritten.ToInt32();
        }

        public byte[] ReadProcessMemory(IntPtr MemoryAddress, uint bytesToRead, out int bytesRead)
        {
            byte[] buffer = new byte[bytesToRead];

            IntPtr ptrBytesRead;
            ProcessMemoryReaderApi.ReadProcessMemory(m_hProcess, MemoryAddress, buffer, bytesToRead, out ptrBytesRead);

            bytesRead = ptrBytesRead.ToInt32();

            return buffer;
        }

        public void WriteProcessMemory(IntPtr MemoryAddress, byte[] bytesToWrite, out int bytesWritten)
        {
            IntPtr ptrBytesWritten;
            ProcessMemoryReaderApi.WriteProcessMemory(m_hProcess, MemoryAddress, bytesToWrite, (uint)bytesToWrite.Length, out ptrBytesWritten);

            bytesWritten = ptrBytesWritten.ToInt32();
        }


        /// <summary>
        /// ProcessMemoryReader is a class that enables direct reading a process memory
        /// </summary>
        class ProcessMemoryReaderApi
        {
            // constants information can be found in <winnt.h>
            [Flags]
            public enum ProcessAccessType
            {
                PROCESS_TERMINATE = (0x0001),
                PROCESS_CREATE_THREAD = (0x0002),
                PROCESS_SET_SESSIONID = (0x0004),
                PROCESS_VM_OPERATION = (0x0008),
                PROCESS_VM_READ = (0x0010),
                PROCESS_VM_WRITE = (0x0020),
                PROCESS_DUP_HANDLE = (0x0040),
                PROCESS_CREATE_PROCESS = (0x0080),
                PROCESS_SET_QUOTA = (0x0100),
                PROCESS_SET_INFORMATION = (0x0200),
                PROCESS_QUERY_INFORMATION = (0x0400)
            }
            public enum flAllocationType
            {
                MEM_COMMIT = 0x00001000,
                MEM_RESET_UNDO = 0x1000000,
                MEM_RESERVE = 0x00002000,
                MEM_RESET = 0x00080000,
                MEM_TOP_DOWN = 0x00100000
            }
            public enum flProtect
            {
                PAGE_EXECUTE = 0x10,
                PAGE_EXECUTE_READ = 0x20,
                PAGE_EXECUTE_READWRITE = 0x40,
                PAGE_EXECUTE_WRITECOPY = 0x80,
                PAGE_NOACCESS = 0x01,
                PAGE_READONLY = 0x02,
                PAGE_READWRITE = 0x04,
                PAGE_WRITECOPY = 0x08,
                PAGE_TARGETS_INVALID = 0x40000000,
                PAGE_TARGETS_NO_UPDATE = 0x40000000
            }
            // function declarations are found in the MSDN and in <winbase.h> 

            //		HANDLE OpenProcess(
            //			DWORD dwDesiredAccess,  // access flag
            //			BOOL bInheritHandle,    // handle inheritance option
            //			DWORD dwProcessId       // process identifier

            //			);
            [DllImport("Kernel32.dll")]
            public static extern IntPtr VirtualAllocEx(IntPtr hProcess, out IntPtr lpAddress, UInt32 dwSize, UInt32 flAllocationType, UInt32 flProtect);
            [DllImport("kernel32.dll")]
            public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);

            //		BOOL CloseHandle(
            //			HANDLE hObject   // handle to object
            //			);
            [DllImport("kernel32.dll")]
            public static extern Int32 CloseHandle(IntPtr hObject);

            //		BOOL ReadProcessMemory(
            //			HANDLE hProcess,              // handle to the process
            //			LPCVOID lpBaseAddress,        // base of memory area
            //			LPVOID lpBuffer,              // data buffer
            //			SIZE_T nSize,                 // number of bytes to read
            //			SIZE_T * lpNumberOfBytesRead  // number of bytes read
            //			);
            [DllImport("kernel32.dll")]
            public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

            //		BOOL WriteProcessMemory(
            //			HANDLE hProcess,                // handle to process
            //			LPVOID lpBaseAddress,           // base of memory area
            //			LPCVOID lpBuffer,               // data buffer
            //			SIZE_T nSize,                   // count of bytes to write
            //			SIZE_T * lpNumberOfBytesWritten // count of bytes written
            //			);
            [DllImport("kernel32.dll")]
            // GG:GF
            public static extern Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesWritten);


        }
    }

}
