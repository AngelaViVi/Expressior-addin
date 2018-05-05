using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shell
{
    public partial class Form1 : Form
    {
        Process _consoleProcess;//控制台的进程

        public Form1()
        {
            InitializeComponent();

            this.Load += new EventHandler((sender, args) =>
            {
                CreateConsoleProcess();
                Thread.Sleep(500);
                this.Activate();
            });

            this.FormClosing += new FormClosingEventHandler((sender, args) =>
            {
                if (_consoleProcess != null)
                {
                    _consoleProcess.Kill();
                }
            });
        }


        private void CreateConsoleProcess()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "powershell";
            //startInfo.Arguments = "python";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;//要重定向IO流,必须设置为false
            //startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.WorkingDirectory = Application.StartupPath;

            _consoleProcess = Process.Start(startInfo);
           

            ThreadPool.QueueUserWorkItem(new WaitCallback(state =>
            {
                while (true)
                {
                    if (_consoleProcess != null && !_consoleProcess.HasExited)
                    {
                        StreamReader sr = _consoleProcess.StandardError;
                        string str = sr.ReadLine();
                        Println(str);
                    }

                    //Thread.Sleep(10);
                }
            }));

            ThreadPool.QueueUserWorkItem(new WaitCallback(state =>
            {
                while (true)
                {
                    if (_consoleProcess != null && !_consoleProcess.HasExited)
                    {
                        StreamReader sr = _consoleProcess.StandardOutput;
                        string str = sr.ReadLine();
                        Println(str);
                    }

                    //Thread.Sleep(10);
                }
            }));
        }
        public void Println(string str)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                if (str != null && str.EndsWith("\n"))
                {
                    richTextBox1.AppendText(str);
                }
                else
                {
                    richTextBox1.AppendText(str + "\n");
                }
                richTextBox1.ScrollToCaret();
            }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _consoleProcess.StandardInput.WriteLine("E:\\VisualStudio\\Expressior-addin\\bin\\x64\\Debug\\testShell.py");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _consoleProcess.StandardInput.WriteLine("1+2");
        }
    }
}
