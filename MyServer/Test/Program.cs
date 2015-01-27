using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        [DllImport("..\\..\\..\\Debug\\ToolKit.dll", ExactSpelling = true, EntryPoint = "?analyze@@YGHPBD@Z", CallingConvention = CallingConvention.StdCall)]
        public static extern int analyze(string fileName);

        static void Main(string[] args)
        {
            if (!File.Exists("..\\..\\..\\Debug\\ToolKit.dll"))
            {
                Console.WriteLine("File Not Exist");
            }
            string fileName = "C:\\Users\\Tony\\Desktop\\1.jpg";
            int result = analyze(fileName);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
