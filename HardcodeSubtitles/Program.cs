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

                cmd.StandardInput.WriteLine(fbd.SelectedPath.Substring(0, 1) + ":");
                cmd.StandardInput.WriteLine("cd " + fbd.SelectedPath.Substring(0, 1) + ":\\");
                cmd.StandardInput.WriteLine("cd " + fbd.SelectedPath);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                Console.WriteLine(cmd.StandardOutput.ReadToEnd());
                string comando = "";
                foreach (var path in Directory.GetFiles(fbd.SelectedPath))
                {
                    comando = "handbrakecli -i \"" + Path.GetFileName(path) + "\" -o \"" +
                        Path.GetFileName(path).Substring(0, Path.GetFileName(path).LastIndexOf(".")) +
                        ".mp4\" -e x264 -N \"por\" --subtitle-burn";
                    new Thread(() => Cmd(comando, fbd.SelectedPath)).Start();
                    Thread.Sleep(500);
                }
                Console.WriteLine(cmd.StandardOutput.ReadToEnd());
                cmd.WaitForExit();
                cmd.Close();
                //Console.Read();
            }
        }
        static void Cmd(string comando, string path)
        {
            Console.Write(comando);
            Process.Start("CMD.exe", "/K " + path.Substring(0,1) + ":&cd " + path + "&" + comando);
        }
    }
}
