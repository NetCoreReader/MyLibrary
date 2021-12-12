using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;

namespace MyLibrary
{
    public sealed class MyNet
    {
        private static readonly Lazy<MyNet> lazy = new Lazy<MyNet>(() => new MyNet());
        public static MyNet Instance { get { return lazy.Value; } }

        private MyNet()
        {

        }

        public string PingGonder(string url)
        {
            string msg = string.Empty;
            Ping p = new Ping();
            Timer t = new Timer();
            t.Interval = 1000;
            t.Start();
            t.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                try
                {
                    PingReply pr = p.Send(url);
                    msg += string.Format("Sonuç: {0}, {1} -> {2} ms.{3}", pr.Status.ToString(), pr.Address.ToString(), pr.RoundtripTime.ToString(), Environment.NewLine);
                }
                catch (PingException ex)
                {
                    msg = "Ping İşlemi Başarısız oldu." + Environment.NewLine + ex.Message;
                }
            };
            System.Threading.Thread.Sleep(5000);
            t.Stop();
            return msg;
        }

        /// <summary>
        /// Kullanımı:
        /// MyNet.InternetGetConnectedStateFlags flag = 0;
        /// bool result = MyNet.InternetGetConnectedState(ref flag, 0);
        /// </summary>
        /// <param name="Description"></param>
        /// <param name="ReservedValue"></param>
        /// <returns></returns>
        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        public static extern bool InternetGetConnectedState(ref InternetGetConnectedStateFlags Description, int ReservedValue);
        [Flags]
        public enum InternetGetConnectedStateFlags
        {
            INTERNET_CONNECTION_MODEM = 0x01,//1, 1
            INTERNET_CONNECTION_LAN = 0x02,//4, 2
            INTERNET_CONNECTION_PROXY = 0x04,//16, 4
            INTERNET_CONNECTION_RAS_INSTALLED = 0x10,//256,16
            INTERNET_CONNECTION_OFFLINE = 0x20,//1024,32
            INTERNET_CONNECTION_CONFIGURED = 0x40//4096, 64
        }

        public string DownloadFileLenght(string urlPath)
        {
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(urlPath.Trim());
            HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
            decimal length = response.ContentLength / 1024; //KB cinsinden boyut öğrenmek için 1024'e böldük.
            response.Close();
            return length.ToString() + " KB";
        }

        public string SetIP(string IPAddress, string Gateway, string SubnetMask = "255.255.255.0")
        {
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();
            foreach (ManagementObject objMO in objMOC)
            {
                if (!(bool)objMO["IPEnabled"])
                    return "IP Adresi Disable";
                try
                {
                    ManagementBaseObject objNewIP = null;
                    ManagementBaseObject objSetIP = null;
                    ManagementBaseObject objNewGate = null;
                    objNewIP = objMO.GetMethodParameters("EnableStatic");
                    objNewGate = objMO.GetMethodParameters("SetGateways");
                    //Varsayılan Ağ Altgeçidi
                    objNewGate["DefaultIPGateway"] = new string[] { Gateway };
                    objNewGate["GatewayCostMetric"] = new int[] { 1 };
                    //IP Adresi ve Ağ alt maskesi
                    objNewIP["IPAddress"] = new string[] { IPAddress };
                    objNewIP["SubnetMask"] = new string[] { SubnetMask };
                    objSetIP = objMO.InvokeMethod("EnableStatic", objNewIP, null);
                    objSetIP = objMO.InvokeMethod("SetGateways", objNewGate, null);
                    return "IP Adresi, SubnetMask ve Default Gateway ayarlandı!";
                }
                catch (Exception ex)
                {
                    return "Hata : " + ex.Message;
                }
            }
            return "İşlem Tamamlandı";
        }

        public string SetDns(string dns1, string dns2)
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string nic = String.Empty;
            foreach (ManagementObject mo in moc)
            {
                if (!(bool)mo["IPEnabled"])
                    return "IP Adresi Disable";
                nic = mo["Caption"].ToString();
                if (mo["Caption"].Equals(nic))
                {
                    ManagementBaseObject DnsEntry = mo.GetMethodParameters("SetDNSServerSearchOrder");
                    string dnsler = dns1 + "," + dns2;
                    DnsEntry["DNSServerSearchOrder"] = dnsler.Split(',');
                    ManagementBaseObject DnsMbo =
                        mo.InvokeMethod("SetDNSServerSearchOrder", DnsEntry, null);
                    int returnCode = int.Parse(DnsMbo["returnvalue"].ToString());
                    if (returnCode == 0)
                        return "Dns adresi değiştirildi.";
                    break;
                }
            }
            return "Dns adresi değiştirilemedi.";
        }

        public string IcIpAdpresi()
        {
            IPHostEntry ipEntry = Dns.GetHostEntry(MyPc.Instance.PcAdi());
            IPAddress[] addr = ipEntry.AddressList;
            string icIp = string.Empty;
            for (int i = 0; i < addr.Length; i++)
                if ((addr[i].ToString().IndexOf(':') > -1) == false)
                    icIp = addr[i].ToString();
            return icIp;
        }

        public string DisIpAdpresi()
        {
            var webClient = new WebClient();
            string disIp = webClient.DownloadString("http://checkip.dyndns.org");
            disIp = (new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b")).Match(disIp).Value;
            webClient.Dispose();
            return disIp;
        }
        public string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            if (context != null)
            {
                string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    string[] addresses = ipAddress.Split(',');
                    if (addresses.Length != 0)
                    {
                        return addresses[0];
                    }
                }

                return context.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
                return "Method Hatası";
        }
        public IPAddress GetIPAddress(string hostName)
        {
            Ping ping = new Ping();
            var replay = ping.Send(hostName);

            if (replay.Status == IPStatus.Success)
            {
                return replay.Address;
            }
            return null;
        }

        public IPStatus IPAddressIsConeect(string hostName)
        {
            Ping ping = new Ping();
            var replay = ping.Send(hostName);
            return replay.Status;
        }
    }
}
