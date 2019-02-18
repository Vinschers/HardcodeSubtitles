using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;

namespace HardcodeSubtitles
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();

                cmd.StandardInput.WriteLine("cd C:\\");
                cmd.StandardInput.WriteLine("cd " + fbd.SelectedPath);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                Console.WriteLine(cmd.StandardOutput.ReadToEnd());
                string comando = "";
                foreach (var path in Directory.GetFiles(fbd.SelectedPath))
                {
                    comando = "HandBrakeCLI -i \"" + Path.GetFileName(path) + "\" -o \"" +
                        Path.GetFileName(path).Substring(0, Path.GetFileName(path).LastIndexOf(".")) +
                        ".mp4\" -e x264 -N \"por\" --subtitle-burn";
                    Console.WriteLine(Path.GetFileName(path)); // file name
                    new Thread(() => Cmd(comando, fbd.SelectedPath)).Start();
                }
                Console.WriteLine(cmd.StandardOutput.ReadToEnd());
                cmd.WaitForExit();
                cmd.Close();
            }
        }
        static void Cmd(string comando, string path)
        {
            //Process cmd = new Process();
            //cmd.StartInfo.FileName = "cmd.exe";
            //cmd.StartInfo.RedirectStandardInput = true;
            //cmd.StartInfo.RedirectStandardOutput = true;
            //cmd.StartInfo.CreateNoWindow = true;
            //cmd.StartInfo.UseShellExecute = false;
            //cmd.Start();

            //cmd.StandardInput.WriteLine("cd C:\\");
            //cmd.StandardInput.WriteLine("cd " + path);
            //cmd.StandardInput.WriteLine(comando);
            //cmd.StandardInput.Flush();
            //cmd.StandardInput.Close();
            //cmd.WaitForExit();
            //Console.WriteLine(cmd.StandardOutput.ReadToEnd());

            Process.Start("CMD.exe", "/K cd C:\\&cd " + path + "&" + comando);

            //Process p = new Process();
            //ProcessStartInfo info = new ProcessStartInfo();
            //info.FileName = "cmd.exe";
            //info.RedirectStandardInput = true;
            //info.UseShellExecute = false;

            //p.StartInfo = info;
            //p.Start();

            //using (StreamWriter sw = p.StandardInput)
            //{
            //    if (sw.BaseStream.CanWrite)
            //    {
            //        sw.WriteLine("cd C:\\");
            //        sw.WriteLine("echo oiiiiiiiii");
            //        sw.WriteLine("echo \"pf funciona\"");
            //    }
            //}
        }
    }
}
