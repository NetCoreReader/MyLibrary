using MyLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailSend
{
    public partial class frmLisanslama : Form
    {
        public frmLisanslama()
        {
            InitializeComponent();
        }

        private void BtnKontrol_Click(object sender, EventArgs e)
        {
            try
            {//188.172.205.191
                var degera = MyCrypto.Instance.SifreyiCoz("6HSUXoWVRSKLOFaY4d/5we+M5MUii07SJK2nqZk0KYtSzjimdvoi+f843nU6hd0nnAdYsbLbYqwzAwu3zAgBjsBeDK7C2GKnV785eB8BIgwn4iFtIweMvYRwleCw2amL7GSvCO8ilG30pNMCgj2+0XFa28djYoY/6Yz7AW55+15JmW1OSBoZNvbiGobupwNAK7SsUIgHeI5IKoO459WC8UUtJlhVH0p9nB45XfH2qLfToNZhsTVUJ83nXCa3D3TSit/AHp2IbPMJE/yOGMRFDuF7gj9kGU0H7Dezv/GNzJMo7ivtTCGeaRyrt6eSaWUxkDBS5u7RFL3uSk/DciB3EtlyS4v0Nuzbzx/1SyTr9dIWL0LeB5EBpUrP+rASsl1ReLkVLxU55tLCUxUfjOQl0RZZ5+VbNTp7MF9bn/ZZUSevh5E2xmZyd26FetSxk/Nsl+Z1MOeA5Fp1sAY8Z7vDNg==", "4890eqwıeo@€£#$");
                var deger2= MyCrypto.Instance.SifreUygula("Data Source=.;Initial Catalog=WWF_DB;Integrated Security=False;Persist Security Info=False;User ID=sa;Password=Logo99x*99xPanda20191903!*;Connect Timeout=60", "4890eqwıeo@€£#$");

                MyConnect.Instance.ExecuteNonQuery(Properties.Settings.Default._cnn, "INSERT INTO X_CATEGORY (ACIKLAMA, KATEGORI_ID) VALUES ('ACIKLAMA', NULL)", CommandType.Text, 60, out MyConnect.Instance.exp);
                var deger = MyConnect.Instance.ExecuteScalar_Int(Properties.Settings.Default._cnn, "select ID from X_CATEGORY where ACIKLAMA = 'ACIKLAMA'", CommandType.Text, 60, out MyConnect.Instance.exp);
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
        }

        private void BtnYeni_Click(object sender, EventArgs e)
        {
            txtKontrol.Text = MyCryptoOthers.Instance.Hash_SHA256(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString());
        }

        private void btnMethod_Click(object sender, EventArgs e)
        {
            lblMethod.Text = MyNet.Instance.GetIPAddress();
        }

        private void btndate_Click(object sender, EventArgs e)
        {
            int obje = MyTools.Instance.TarihiSayiya(new DateTime(2021, 8, 19));
            var obje2 = MyTools.Instance.SayiyiTarihe(obje);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyLibrary.MyLog4Net.Helper.Instance.LogType = MyLibrary.MyLog4Net.Helper.MyLogType.SmtpAppender;
            MyLibrary.MyLog4Net.Helper.Instance.SmtpConfig = new MyLibrary.MyLog4Net.clsSmtpAppenderConfig()
            {
                _Auth = MyLibrary.MyLog4Net.Auth.Basic,
                Port = 587,
                BufferSize = 1,
                From = "a.elikara026@gmail.com",
                Lossy = false,
                Name = "Deneme Aş",
                Priority = System.Net.Mail.MailPriority.High,
                Password = "nlhixikdaxsltceg",
                SmtpHost = "smtp.gmail.com",
                Ssl = true,
                Subject = "Mail Konusu",
                UserName = "a.elikara026@gmail.com",
                To = "a.elikara026@gmail.com",
                Cc = "a.elikara026@gmail.com",
                Bcc = "a.elikara026@gmail.com"
            };
            MyLibrary.MyLog4Net.Helper.Instance.CreateLog();
            MyLibrary.MyLog4Net.Helper.Instance.MyLog(typeof(frmLisanslama), MyLibrary.MyLog4Net.Helper.MyLogLevel.Fatal, "dENEME hATASI");
            MyLibrary.MyLog4Net.Helper.Instance.MyLog(typeof(frmLisanslama), MyLibrary.MyLog4Net.Helper.MyLogLevel.Warn, "2. Deneme hATASI");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MyLibrary.MyLog4Net.Helper.Instance.LogType = MyLibrary.MyLog4Net.Helper.MyLogType.RollingFileAppender;
            MyLibrary.MyLog4Net.Helper.Instance.CreateLog();
            MyLibrary.MyLog4Net.Helper.Instance.MyLog(typeof(frmLisanslama), MyLibrary.MyLog4Net.Helper.MyLogLevel.Fatal, "Hata 1");
            MyLibrary.MyLog4Net.Helper.Instance.MyLog(typeof(frmLisanslama), MyLibrary.MyLog4Net.Helper.MyLogLevel.Error, "Hata 2");


        }

        private void button3_Click(object sender, EventArgs e)
        {
            //String ss = "Deneme";
            //MyEnum aa = MyExtension.ParseEnum<MyEnum>(ss);
            Enum.TryParse("xxx", out MyEnum myStatus);

        }

        enum MyEnum
        {
            deneme,
            hata
        }
    }
}
