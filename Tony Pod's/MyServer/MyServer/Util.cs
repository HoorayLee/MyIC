using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MyServer
{
    class Util
    {
        [DllImport("..\\..\\..\\Debug\\ToolKit.dll", ExactSpelling = true, EntryPoint = "?analyze@@YGHPBD@Z", CallingConvention = CallingConvention.StdCall)]
        public static extern int analyze(string fileName);

    }
}
