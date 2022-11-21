using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Stable_Diffusion_UI
{
    public static class ScriptLauncher
    {
        public static Process p = null;
        private static OutputParser outputParser = new OutputParser();
        public static void RunScript(string rpath, string scriptpath, string arguments, bool RedirectStandardOutput, bool shellexc)
        {
            try
            {
                var info = new ProcessStartInfo
                {
                    FileName = rpath,
                    WorkingDirectory = Path.GetDirectoryName(scriptpath),
                    Arguments = " /c \"" +scriptpath+"\"",
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                using (var proc = new Process { StartInfo = info })
                {
                    p=proc; // Assign so we can dispose of this later
                    proc.OutputDataReceived += Proc_OutputDataReceived;
                    //proc.ErrorDataReceived += Proc_ErrorDataReceived;
                    proc.Start();
                    //proc.BeginErrorReadLine();
                    proc.BeginOutputReadLine();
                    //proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        private static void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            //If object is removed prior to closing
            try
            {
                if (e.Data != null)
                {
                    Task.Run(() => outputParser.parseString(e.Data.ToString()));
                    //string tmp = e.Data.ToString();
                    //if (e.Data.Contains("Stable Diffusion is ready!"))
                    //{
                        //Task.Run(() => ControlsInvoker.hideStartupWindow());
                    //}
                    //else
                    //{
                        //Task.Run(() => ControlsInvoker.updateStartup("Loading: " + tmp));
                        //Task.Run(() => ControlsInvoker.UpdateUIconsole(tmp));
                        //Console.Out.WriteLine(e.Data);
                    //}
                      //Task.Run(() => ControlsInvoker.UpdateUIconsole(tmp));
                }
            }
            catch
            {

            }
            
        }
    }
}
