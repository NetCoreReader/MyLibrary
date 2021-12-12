using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management;
using System.Security.Principal;

namespace MyLibrary
{
    public sealed class MyTools
    {
        private static Lazy<MyTools> lazy = new Lazy<MyTools>(() => new MyTools());

        public static MyTools Instance { get { return lazy.Value; } }

        private MyTools()
        {

        }

        #region Classlar
        public sealed class FizikselDosya
        {
            public string DosyaAdi { get; set; }
            public string DosyaYolu { get; set; }
        }
        public class DosyaGonderimParametresi
        {
            public string DestinationPath { get; set; }
            public string FilePath { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }
        }
        #endregion

        /// <summary>
        /// Klasör Yoksa Oluşturur Var İse Birşey Yapmaz
        /// </summary>
        /// <param name="Path"></param>
        public void CreateDirectory(string Path)
        {
            if (String.IsNullOrEmpty(Path) || String.IsNullOrWhiteSpace(Path))
                throw new Exception("Klasör Adı Boş Olamaz");
            DirectoryInfo dInfo = new DirectoryInfo(Path);
            if (dInfo.Exists == false)
                try
                {
                    dInfo.Create();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
        }

        public int[] FabionacciDizisi(int adimSayisi, ref double altinOran)
        {
            int[] numArray = new int[adimSayisi];
            numArray[0] = 0;
            numArray[1] = 1;
            for (int i = 2; i < adimSayisi; i++)
            {
                numArray[i] = numArray[i - 1] + numArray[i - 2];
            }
            altinOran = ((double)numArray[adimSayisi - 1]) / ((double)numArray[adimSayisi - 2]);
            return numArray;
        }

        public void FileIsRunAsAdmin(string filePath, ref string msg)
        {
            FileInfo info = new FileInfo(filePath);
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = info.DirectoryName,
                FileName = info.FullName,
                Verb = "runas"
            };
            try
            {
                Process.Start(startInfo);
            }
            catch (Exception exception)
            {
                msg = exception.Message;
            }
        }

        public List<FizikselDosya> FizikselDosyalariGetir(string dizin)
        {
            List<FizikselDosya> list = new List<FizikselDosya>();
            foreach (string str in Directory.GetFiles(dizin))
            {
                FizikselDosya item = new FizikselDosya
                {
                    DosyaAdi = str.Substring(str.LastIndexOf(@"\")),
                    DosyaYolu = str
                };
                list.Add(item);
            }
            return list;
        }

        public DataTable ListToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor descriptor in properties)
                table.Columns.Add(descriptor.Name, Nullable.GetUnderlyingType(descriptor.PropertyType) ?? descriptor.PropertyType);
            foreach (T local in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor descriptor2 in properties)
                    row[descriptor2.Name] = descriptor2.GetValue(local) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public DataTable DataTableWhere(DataTable _dt, string Where)
        {
            DataTable _dt1 = _dt.Clone();
            foreach (DataRow item in _dt.Select(Where))
                _dt1.ImportRow(item);
            return _dt1;
        }

        public int HaftaSayisi(DateTime dtPassed)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }

        public bool ListToTxtFile(string filePath, List<string> liste, ref string HataMesaji)
        {
            bool flag3;
            try
            {
                if (Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        for (int i = 0; i < liste.Count; i++)
                            writer.WriteLine(liste[i]);
                        writer.Close();
                        return true;
                    }
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    using (StreamWriter writer2 = new StreamWriter(filePath))
                    {
                        for (int j = 0; j < liste.Count; j++)
                            writer2.WriteLine(liste[j]);
                        writer2.Close();
                        flag3 = true;
                    }
                }
            }
            catch (Exception exception)
            {
                HataMesaji = exception.Message;
                flag3 = false;
            }
            return flag3;
        }

        public string OndalikSayiYuvarla(decimal deger, int virgulSonrasi)
        {
            string format = "{0:0.";
            for (int i = 0; i < virgulSonrasi; i++)
            {
                format = format + "0";
            }
            format = (virgulSonrasi == 0) ? (format.Replace(".", "") + "}") : (format + "}");
            return string.Format(format, deger);
        }

        public string OndalikSperator { get { return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator; } }

        public string OndalikYanSperator { get { return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == "." ? "," : "."; } }

        public string SayiyiYaziyaCevirTR(decimal tutar)
        {
            string str = tutar.ToString("F2").Replace('.', ',');
            string str2 = str.Substring(0, str.IndexOf(','));
            string str3 = str.Substring(str.IndexOf(',') + 1, 2);
            string str4 = "";
            string[] strArray = new string[] { "", "BİR", "İKİ", "\x00dc\x00e7", "D\x00d6RT", "BEŞ", "ALTI", "YEDİ", "SEKİZ", "DOKUZ" };
            string[] strArray2 = new string[] { "", "ON", "YİRMİ", "OTUZ", "KIRK", "ELLİ", "ALTMIŞ", "YETMİŞ", "SEKSEN", "DOKSAN" };
            string[] strArray3 = new string[] { "KATRİLYON", "TRİLYON", "MİLYAR", "MİLYON", "BİN", "" };
            int num = 6;
            str2 = str2.PadLeft(num * 3, '0');
            for (int i = 0; i < (num * 3); i += 3)
            {
                string str5 = "";
                if (str2.Substring(i, 1) != "0")
                {
                    str5 = str5 + strArray[Convert.ToInt32(str2.Substring(i, 1))] + "Y\x00dcZ";
                }
                if (str5 == "BİRY\x00dcZ")
                {
                    str5 = "Y\x00dcZ";
                }
                str5 = str5 + strArray2[Convert.ToInt32(str2.Substring(i + 1, 1))] + strArray[Convert.ToInt32(str2.Substring(i + 2, 1))];
                if (str5 != "")
                {
                    str5 = str5 + strArray3[i / 3];
                }
                if (str5 == "BİRBİN")
                {
                    str5 = "BİN";
                }
                str4 = str4 + str5;
            }
            if (str4 != "")
            {
                str4 = str4 + " TL ";
            }
            int length = str4.Length;
            if (str3.Substring(0, 1) != "0")
            {
                str4 = str4 + strArray2[Convert.ToInt32(str3.Substring(0, 1))];
            }
            if (str3.Substring(1, 1) != "0")
            {
                str4 = str4 + strArray[Convert.ToInt32(str3.Substring(1, 1))];
            }
            if (str4.Length > length)
            {
                return (str4 + " Krş.");
            }
            return (str4 + "SIFIR Krş.");
        }

        public string SayiyiYaziyaCevirENG(decimal tutar)
        {
            string str = tutar.ToString("F2").Replace('.', ',');
            string str2 = str.Substring(0, str.IndexOf(','));
            string str3 = str.Substring(str.IndexOf(',') + 1, 2);
            string str4 = "";
            string[] strArray = new string[] { "", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE" };
            string[] strArray2 = new string[] { "", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };
            string[] strArray3 = new string[] { "KATRİLYON", "TRİLYON", "MİLYAR", "MİLYON", "BİN", "" };
            int num = 6;
            str2 = str2.PadLeft(num * 3, '0');
            for (int i = 0; i < (num * 3); i += 3)
            {
                string str5 = "";
                if (str2.Substring(i, 1) != "0")
                {
                    str5 = str5 + strArray[Convert.ToInt32(str2.Substring(i, 1))] + "HUNDRED";
                }
                if (str5 == "ONE HUNDRED")
                {
                    str5 = "HUNDRED";
                }
                str5 = str5 + strArray2[Convert.ToInt32(str2.Substring(i + 1, 1))] + strArray[Convert.ToInt32(str2.Substring(i + 2, 1))];
                if (str5 != "")
                {
                    str5 = str5 + strArray3[i / 3];
                }
                if (str5 == "ONE THOUSAND")
                {
                    str5 = "THOUSAND";
                }
                str4 = str4 + str5;
            }
            if (str4 != "")
            {
                str4 = str4 + " EUR ";
            }
            int length = str4.Length;
            if (str3.Substring(0, 1) != "0")
            {
                str4 = str4 + strArray2[Convert.ToInt32(str3.Substring(0, 1))];
            }
            if (str3.Substring(1, 1) != "0")
            {
                str4 = str4 + strArray[Convert.ToInt32(str3.Substring(1, 1))];
            }
            if (str4.Length > length)
            {
                return (str4 + " CENT.");
            }
            return (str4 + "ZERO CENT.");
        }

        public DateTime FirstWeekDay
        {
            get
            {
                int dayOfWeek = (int)DateTime.Now.DayOfWeek;
                if (dayOfWeek == 0)
                    return DateTime.Now.AddDays(-6.0);
                return DateTime.Now.AddDays((double)(1 - dayOfWeek))._ToDateTimeOnly();
            }
        }

        public DateTime LastWeekDay
        {
            get { return FirstWeekDay.AddDays(6.0)._ToDateTimeOnly(); }
        }

        public void AgdakiBilgisayaraDosyaKopyala(DosyaGonderimParametresi param, ref string errorMsg)
        {
            try
            {
                AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                WindowsImpersonationContext context = new WindowsIdentity(param.UserName, param.Password).Impersonate();
                File.Copy(param.FilePath, param.DestinationPath, true);
                context.Undo();
            }
            catch (Exception exception)
            {
                errorMsg = exception.Message;
            }
        }

        public int cmdKomutuCalistir(string command, int timeout)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe", "/C " + command)
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = @"C:\\"
            };
            Process process = Process.Start(startInfo);
            process.WaitForExit(timeout);
            int exitCode = process.ExitCode;
            process.Close();
            return exitCode;
        }

        public OleDbDataReader ExcelDataGetir(string dosyaYolu, string sayfaIsmi, ref string gelenMesaj)
        {
            OleDbConnection connection = new OleDbConnection();
            if (Path.GetExtension(dosyaYolu) == ".xls")
            {
                connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dosyaYolu + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"");
            }
            else if (Path.GetExtension(dosyaYolu) == ".xlsx")
            {
                connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dosyaYolu + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"");
            }
            OleDbCommand command = new OleDbCommand(string.Format("Select * From [{0}$]", sayfaIsmi), connection);
            try
            {
                connection.Open();
            }
            catch (Exception exception)
            {
                gelenMesaj = exception.Message;
                return null;
            }
            OleDbDataReader reader = command.ExecuteReader();
            connection.Close();
            return reader;
        }

        public enum MyPcSorguListesi
        {
            CPUSeriNoCek,
            HDDSeriNoCekListeOlarak,
            YazicilariGetir,
            AktifKullanici
        }

        public ManagementObjectCollection MyPcVeriGetir(MyPcSorguListesi sorguCumlesi)
        {
            List<string> list = new List<string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(SorguGetir(sorguCumlesi));
            return searcher.Get();
        }

        public string SorguGetir(MyPcSorguListesi liste)
        {
            string str = "";
            switch (liste)
            {
                case MyPcSorguListesi.CPUSeriNoCek:
                    return "Select * FROM WIN32_Processor";

                case MyPcSorguListesi.HDDSeriNoCekListeOlarak:
                    return str;

                case MyPcSorguListesi.YazicilariGetir:
                    return str;

                case MyPcSorguListesi.AktifKullanici:
                    return str;
            }
            return str;
        }

        public int SaatiSayiya(int saat, int dakika, int saniye)
        {
            return saat * 65536 * 256 + dakika * 65536 + saniye * 256;
        }

        public string SayiyiSaate(int sayi)
        {
            int hh = 0, mm = 0, ss = 0;
            hh = (sayi - (sayi % 65536)) / 65536 / 256;
            mm = (sayi - (sayi % 65536)) / 65536 - ((sayi - (sayi % 65536)) / 65536 / 256) * 256;
            ss = ((sayi % 65536) - ((sayi % 65536) % 256)) / 256;
            string time = $"{hh.ToString().PadLeft(2, '0')}:{mm.ToString().PadLeft(2, '0')}:{ss.ToString().PadLeft(2, '0')}";
            return time;
        }

        public int TarihiSayiya(DateTime _dt)
        {
            return _dt.Year * 65536 + _dt.Month * 256 + _dt.Day;
        }

        public DateTime SayiyiTarihe(int sayi)
        {
            int dd = 0, mm = 0, yy = 0;
            dd = (sayi % 65536) % 256;
            mm = (sayi % 65536) / 256;
            yy = (sayi / 65536);
            return new DateTime(yy, mm, dd);
        }

        public byte[] GetImagePathToByte(string resimYolu)
        {
            byte[] resim = null;
            FileStream fs = new FileStream(resimYolu, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            resim = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();
            return resim;
        }

        public byte[] GetImageToByte(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }

        public System.Drawing.Image GetByteToImage(byte[] gelenImage)
        {
            System.Drawing.Image img = null;
            if (gelenImage != null)
                using (var ms = new MemoryStream(gelenImage))
                    img = System.Drawing.Image.FromStream(ms);
            return img;
        }

        public string SeriNoOlustur(string numara)
        {
            if (string.IsNullOrEmpty(numara) || string.IsNullOrWhiteSpace(numara))
                throw new Exception("Numara Boş Geçilemez.");
            bool hata = true;
            foreach (char item in numara)
                if (char.IsDigit(item))
                {
                    hata = false;
                    break;
                }
            if (hata)
                throw new Exception("Numara İçerinde Rakam Olmalı.");
            List<string> sayi = new List<string>();
            int gecilenIndex = -1;
            bool sayiHafiza = true;
            bool harfHafiza = true;
            foreach (char item in numara)
            {
                if (char.IsDigit(item))
                {
                    harfHafiza = true;
                    if (sayiHafiza)
                    {
                        sayi.Add(item.ToString());
                        gecilenIndex++;
                        sayiHafiza = false;
                    }
                    else
                        sayi[gecilenIndex] += item.ToString();
                }
                else
                {
                    sayiHafiza = true;
                    if (harfHafiza)
                    {
                        sayi.Add(item.ToString());
                        gecilenIndex++;
                        harfHafiza = false;
                    }
                    else
                        sayi[gecilenIndex] += item.ToString();
                }
            }
            for (int i = 0; i < sayi.Count; i++)
            {
                try
                {
                    int sayma = Convert.ToInt32(sayi[sayi.Count - 1 - i]);
                    sayi[sayi.Count - 1 - i] = (++sayma).ToString().PadLeft(sayi[sayi.Count - 1 - i].Length, '0');
                    break;
                }
                catch (Exception)
                {

                }
            }
            string gonder = string.Empty;
            for (int i = 0; i < sayi.Count; i++)
                gonder += sayi[i];
            return gonder;
        }

        //public static void AlertGetir(Form frm, string baslik, string uyari, bool sag)
        //{
        //    AlertControl _Alert = new AlertControl();
        //    if (!sag)
        //        _Alert.ControlBoxPosition = AlertFormControlBoxPosition.Top;
        //    _Alert.LookAndFeel.UseDefaultLookAndFeel = false;
        //    _Alert.LookAndFeel.SkinName = "Blue";  //Office 2016 Colorful
        //    _Alert.Show(frm, baslik, uyari);
        //}

        //public static void AlertGetirResimli(Form frm, string baslik, string uyari,Image resim,bool sag)
        //{
        //    AlertControl _Alert = new AlertControl();
        //    //AlertButton btn = new AlertButton(Properties.Resources.icn16_Warning);
        //    //_Alert.Buttons.Add(btn);
        //    if (!sag)
        //        _Alert.ControlBoxPosition = AlertFormControlBoxPosition.Top;
        //    _Alert.LookAndFeel.UseDefaultLookAndFeel = false;
        //    _Alert.LookAndFeel.SkinName = sag ? "Blue" : "Office 2016 Colorful";
        //    _Alert.Show(frm, baslik, uyari,resim);
        //    /*using DevExpress.XtraBars.Alerter;
        //                        // Create a regular custom button. 
        //        AlertButton btn1 = new AlertButton(Image.FromFile(@"c:\folder-16x16.png"));
        //        btn1.Hint = "Open file";
        //        btn1.Name = "buttonOpen";
        //        // Create a check custom button. 
        //        AlertButton btn2 = new AlertButton(Image.FromFile(@"c:\clock-16x16.png"));
        //        btn2.Style = AlertButtonStyle.CheckButton;
        //        btn2.Down = true;
        //        btn2.Hint = "Alert On";
        //        btn2.Name = "buttonAlert";
        //        // Add buttons to the AlertControl and subscribe to the events to process button clicks 
        //        alertControl1.Buttons.Add(btn1);
        //        alertControl1.Buttons.Add(btn2);
        //        alertControl1.ButtonClick += new AlertButtonClickEventHandler(alertControl1_ButtonClick);
        //        alertControl1.ButtonDownChanged += 
        //            new AlertButtonDownChangedEventHandler(alertControl1_ButtonDownChanged);
                
        //        // Show a sample alert window. 
        //        AlertInfo info = new AlertInfo("New Window", "Text");
        //        alertControl1.Show(this, info);
                
        //        void alertControl1_ButtonDownChanged(object sender, 
        //        AlertButtonDownChangedEventArgs e) {
        //            if (e.ButtonName == "buttonOpen") {
        //                //... 
        //            }
        //        }
                
        //        void alertControl1_ButtonClick(object sender, AlertButtonClickEventArgs e) {
        //            if (e.ButtonName == "buttonAlert") {
        //                //... 
        //            }
        //        }
        //            */

        //}
    }

}

