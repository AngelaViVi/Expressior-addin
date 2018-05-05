using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealShell
{
    class Program
    {
        public string DelegateShell(string pyPath,string filepath,string Params=null)
        {
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo(pyPath, filepath + Params);
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(procStartInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    return result;
                }
            }
        }


        static void Main(string[] args)
        {
            Program mProgram =new Program();
            //string result=mProgram.DelegateShell("D:\\Libraries\\Anaconda3\\python.exe",
            //    "E:\\VisualStudio\\Expressior-addin\\bin\\x64\\Debug\\testShell.py");
            string result = mProgram.DelegateShell("powershell"," D:\\Libraries\\Anaconda3\\python.exe",
                " E:\\VisualStudio\\Expressior-addin\\bin\\x64\\Debug\\testShell.py");

            Console.WriteLine("请看法宝:"+result);



        }
    }
}
