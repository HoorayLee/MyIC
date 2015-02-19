using Doctor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doctor
{
    class LoginStatus
    {
        public delegate void LoginStatusChangedEventHandler(EventArgs e);

        public static event LoginStatusChangedEventHandler LoginStatusChanged;

        private static void OnLoginStatusChanged(EventArgs e)
        {
            if (LoginStatusChanged != null)
            {
                LoginStatusChanged(e);
            }
        }

        private static DoctorModel userInfo;

        public static DoctorModel UserInfo
        {
            get
            {
                return userInfo;
            }
            set
            {
                userInfo = value;
                OnLoginStatusChanged(EventArgs.Empty);
            }
        }
        public static void Clear() 
        { 
            UserInfo = null;
            OnLoginStatusChanged(EventArgs.Empty);
        }
    }
}
