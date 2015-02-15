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
        [DllImport("ToolKit.dll", ExactSpelling = true, EntryPoint = "?analyze@@YG?AU_Illnesses@@PBD@Z", CallingConvention = CallingConvention.StdCall)]
        public static extern Illnesses analyze(string fileName);

        [StructLayout(LayoutKind.Sequential)]
        public struct Illnesses
        {
            public int nbTeeth;
            public int status;
            public IntPtr illnesses;
        }
    }
}
