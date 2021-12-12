using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MyLibrary
{
    public class MyPc
    {
        private static Lazy<MyPc> lazy = new Lazy<MyPc>(() => new MyPc());

        public static MyPc Instance { get { return lazy.Value; } }

        private MyPc()
        {

        }
        public string PcAdi()
        {
            return Dns.GetHostName();
        }
        public string Mac()
        {
            ManagementClass manager = new ManagementClass("Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject obj in manager.GetInstances())
                if ((bool)obj["IPEnabled"])
                    return obj["MacAddress"].ToString();
            return String.Empty;
        }
    }
}
