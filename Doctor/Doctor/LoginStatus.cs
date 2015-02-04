using Doctor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doctor
{
    class LoginStatus
    {
        public static DoctorModel UserInfo { get; set; }
        public static void Clear() { UserInfo = null; }
    }
}
