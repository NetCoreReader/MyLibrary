using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MyLibrary
{
    public sealed class MyCryptoOthers
    {
        private static Lazy<MyCryptoOthers> lazy = new Lazy<MyCryptoOthers>(() => new MyCryptoOthers());

        public static MyCryptoOthers Instance { get { return lazy.Value; } }

        private MyCryptoOthers()
        {

        }

        private static byte[] ByteDonustur(string deger)
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            return ByteConverter.GetBytes(deger);
        }

        private static byte[] Byte8(string deger)
        {
            char[] arrayChar = deger.ToCharArray();
            byte[] arrayByte = new byte[arrayChar.Length];
            for (int i = 0; i < arrayByte.Length; i++)
                arrayByte[i] = Convert.ToByte(arrayChar[i]);
            return arrayByte;
        }

        #region Hash Şifreleme
        /*
        * Hash Şifreleme
        * Şifrelenen veriye tekrar ulaşmanız mümkün değildir. 
        * Şöyleki varsayalım 1000 karakterlik bir veriyi md5, sha 1, sha256 veya sha512 ile şifrelediniz bu durumda elde edeceğiniz şifrelenmiş veri algoritma türüne göre standart boyutta bir karakter katarı olacaktır.
        */
        public string Hash_MD5(string data)
        {
            if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException("Şifrelenecek veri yok");
            else
            {
                MD5CryptoServiceProvider sifre = new MD5CryptoServiceProvider();
                byte[] arySifre = ByteDonustur(data);
                byte[] aryHash = sifre.ComputeHash(arySifre);
                return BitConverter.ToString(aryHash);
            }
        }

        public string Hash_SHA1(string data)
        {
            if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException("Şifrelenecek veri yok");
            else
            {
                SHA1CryptoServiceProvider sifre = new SHA1CryptoServiceProvider();
                byte[] arySifre = ByteDonustur(data);
                byte[] aryHash = sifre.ComputeHash(arySifre);
                return BitConverter.ToString(aryHash);
            }
        }

        public string Hash_SHA256(string data)
        {
            if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException("Şifrelenecek veri yok");
            else
            {
                SHA256Managed sifre = new SHA256Managed();
                byte[] arySifre = ByteDonustur(data);
                byte[] aryHash = sifre.ComputeHash(arySifre);
                return BitConverter.ToString(aryHash);
            }
        }

        public string Hash_SHA384(string data)
        {
            if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException("Şifrelenecek veri yok");
            else
            {
                SHA384Managed sifre = new SHA384Managed();
                byte[] arySifre = ByteDonustur(data);
                byte[] aryHash = sifre.ComputeHash(arySifre);
                return BitConverter.ToString(aryHash);
            }
        }

        public string Hash_SHA512(string data)
        {
            if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException("Şifrelenecek veri yok");
            else
            {
                SHA512Managed sifre = new SHA512Managed();
                byte[] arySifre = ByteDonustur(data);
                byte[] aryHash = sifre.ComputeHash(arySifre);
                return BitConverter.ToString(aryHash);
            }
        }
        #endregion

        #region Simetrik Şifreleme
        /*
         * Simetrik şifreleme yöntemlerinde şifrelenen veriye ulaşmak için şifreyi çözen tarafın veriyi şifreleyen tarafla aynı anahtara ihtiyacı vardır. 
         */
        /// <summary>
        /// DES algoritması 8 bit anahtar ve 8 bit iv değeri kullanır
        /// </summary>
        /// <param name="data">Şifrelenecek Metin</param>
        /// <param name="_8bit_1">8 Karakterlik Birinci Veri Anahtarı Giriniz.</param>
        /// <param name="_8bit_2">8 Karakterlik İkinci Veri Anahtarı Giriniz.</param>
        /// <returns>string</returns>
        public string Simetrik_DESSifrele(string data, string _8bit_1, string _8bit_2)
        {
            string result = "";
            if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException("Şifrelenecek veri yok");
            else
            {
                byte[] aryKey = Byte8(_8bit_1);
                byte[] aryIV = Byte8(_8bit_2);
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateEncryptor(aryKey, aryIV), CryptoStreamMode.Write);
                StreamWriter writer = new StreamWriter(cs);
                writer.Write(data);
                writer.Flush();
                cs.FlushFinalBlock();
                writer.Flush();
                result = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
                writer.Dispose();
                cs.Dispose();
                ms.Dispose();
            }
            return result;
        }
        /// <summary>
        /// DES algoritması 8 bit anahtar ve 8 bit iv değeri kullanır
        /// </summary>
        /// <param name="_8bit_1">8 Karakterlik Birinci Veri Anahtarı Giriniz.</param>
        /// <param name="_8bit_2">8 Karakterlik İkinci Veri Anahtarı Giriniz.</param>
        /// <returns>string</returns>
        public string Simetrik_DESCoz(string data, string _8bit_1, string _8bit_2)
        {
            string result = "";
            if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException("Şifrelenecek veri yok");
            else
            {
                byte[] aryKey = Byte8(_8bit_1);
                byte[] aryIV = Byte8(_8bit_2);
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream(Convert.FromBase64String(data));
                CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateDecryptor(aryKey, aryIV), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cs);
                result = reader.ReadToEnd();
                reader.Dispose();
                cs.Dispose();
                ms.Dispose();
            }
            return result;
        }
        #region TripleDES Açıklaması
        //Çift yönlü çalışır.Şifrelenmiş veri geri çözülebilir.
        //DES şifrelemesinin 3 kere art arda yapılması şeklinde çalışır.
        //DES şifreleme yöntemine göre 3 kat daha yavaş çalışır.
        //Şifreleme yapmak için uzunluğu 24 bayt olan bir anahtar kullanılır.Her bayt için 1 eşlik biti vardır.Dolayısıyla anahtarın uzunluğu 168 bittir.
        //Veri, 3DES anahtarının ilk 8 baytı ile şifrelenir.
        //Sonra veri anahtarın ortadaki 8 baytı ile çözülür.
        //Son olarak anahtarın son 8 baytı ile şifrelenerek 8 bayt bir blok elde edilir.
        ///Avantajları:
        //Çift yönlü çalıştığından şifreli bir şekilde veriler saklanabilir, istenildiği zaman geri çağrılarak şifresi çözülebilir.
        //Bilgisayarın donanımsal açıklarını kapatır. (örnek: VPN, veri haberleşme ağları)
        ///Dezavantajları:
        //Güvenlik tamamen kullanılan anahtara dayanmaktadır.Anahtarın zayıflığı, şifrenin çözülmesini kolaylaştırır.
        //Daha gelişmiş bir algoritmaya sahip olan AES (Advanced Encryption Standard-Gelişmiş Şifreleme Standardı) şifreleme yöntemine göre 6 kat daha yavaş çalışır.
        #endregion
        public string Simetrik_TripleDESSifrele(string PlainText, string key)
        {
            TripleDES des = CreateDES(key);
            ICryptoTransform ct = des.CreateEncryptor();
            byte[] input = Encoding.Unicode.GetBytes(PlainText);
            return Convert.ToBase64String(ct.TransformFinalBlock(input, 0, input.Length));
        }
        public string Simetrik_TripleDESCoz(string CypherText, string key)
        {
            byte[] b = Convert.FromBase64String(CypherText);
            TripleDES des = CreateDES(key);
            ICryptoTransform ct = des.CreateDecryptor();
            byte[] output = ct.TransformFinalBlock(b, 0, b.Length);
            return Encoding.Unicode.GetString(output);
        }
        private TripleDES CreateDES(string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Key = md5.ComputeHash(Encoding.Unicode.GetBytes(key));
            des.IV = new byte[des.BlockSize / 8];
            return des;
        }




        /// <summary>
        /// RC2 algoritması 8 bit anahtar ve 8 bit iv değeri kullanır.
        /// </summary>
        /// <param name="data">Şifrelenecek Metin</param>
        /// <param name="_8bit_1">8 karakterlik birinci anahtar</param>
        /// <param name="_8bit_2">8 karakterlik ikinci anahtar</param>
        /// <returns>string</returns>
        public string Simetrik_RC2Sifrele(string data, string _8bit_1, string _8bit_2)
        {
            string result = "";
            if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException("Şifrelenecek veri yok");
            else
            {
                byte[] aryKey = Byte8(_8bit_1);
                byte[] aryIV = Byte8(_8bit_2);
                RC2CryptoServiceProvider dec = new RC2CryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, dec.CreateEncryptor(aryKey, aryIV), CryptoStreamMode.Write);
                StreamWriter writer = new StreamWriter(cs);
                writer.Write(data);
                writer.Flush();
                cs.FlushFinalBlock();
                writer.Flush();
                result = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
                writer.Dispose();
                cs.Dispose();
                ms.Dispose();
            }
            return result;
        }
        /// <summary>
        /// RC2 algoritması 8 bit anahtar ve 8 bit iv değeri kullanır.
        /// </summary>
        /// <param name="data">Şifrelenmiş Metin</param>
        /// <param name="_8bit_1">8 karakterlik birinci anahtar</param>
        /// <param name="_8bit_2">8 karakterlik ikinci anahtar</param>
        /// <returns>string</returns>
        public string Simetrik_RC2Coz(string data, string _8bit_1, string _8bit_2)
        {
            string result = "";
            if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException("Şifrelenecek veri yok");
            else
            {
                byte[] aryKey = Byte8(_8bit_1);
                byte[] aryIV = Byte8(_8bit_2);
                RC2CryptoServiceProvider cp = new RC2CryptoServiceProvider();
                MemoryStream ms = new MemoryStream(Convert.FromBase64String(data));
                CryptoStream cs = new CryptoStream(ms, cp.CreateDecryptor(aryKey, aryIV), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cs);
                result = reader.ReadToEnd();
                reader.Dispose();
                cs.Dispose();
                ms.Dispose();
            }
            return result;
        }
        /// <summary>
        /// Rijndael algoritması 8 anahtar ve 16 bit iv değeri kullanır.
        /// </summary>
        /// <param name="data">Şifrelenecek Metin</param>
        /// <param name="_8bit">8 karakterlik birinci anahtar</param>
        /// <param name="_16bit">16 karakterlik ikinci anahtar</param>
        /// <returns>string</returns>
        public string Simetrik_RijndaelSifrele(string data, string _8bit, string _16bit)
        {
            string result = "";
            if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException("Şifrelenecek veri yok");
            else
            {
                byte[] aryKey = Byte8(_8bit);
                byte[] aryIV = Byte8(_16bit);
                RijndaelManaged dec = new RijndaelManaged();
                dec.Mode = CipherMode.CBC;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, dec.CreateEncryptor(aryKey, aryIV), CryptoStreamMode.Write);
                StreamWriter writer = new StreamWriter(cs);
                writer.Write(data);
                writer.Flush();
                cs.FlushFinalBlock();
                writer.Flush();
                result = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
                writer.Dispose();
                cs.Dispose();
                ms.Dispose();
            }
            return result;
        }
        /// <summary>
        /// Rijndael algoritması 8 anahtar ve 16 bit iv değeri kullanır.
        /// </summary>
        /// <param name="data">Şifrelenmiş Metin</param>
        /// <param name="_8bit">8 karakterlik birinci anahtar</param>
        /// <param name="_16bit">16 karakterlik ikinci anahtar</param>
        /// <returns>string</returns>
        public string Simetrik_RijndaelCoz(string data, string _8bit, string _16bit)
        {
            string result = "";
            if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException("Şifrelenecek veri yok");
            else
            {
                byte[] aryKey = Byte8(_8bit);
                byte[] aryIV = Byte8(_16bit);
                RijndaelManaged cp = new RijndaelManaged();
                MemoryStream ms = new MemoryStream(Convert.FromBase64String(data));
                CryptoStream cs = new CryptoStream(ms, cp.CreateDecryptor(aryKey, aryIV), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cs);
                result = reader.ReadToEnd();
                reader.Dispose();
                cs.Dispose();
                ms.Dispose();
            }
            return result;
        }
        #endregion

        #region Asimetrik Şifreleme
        /*
         * Asimetrik şifreleme yöntemleri ileri düzey şifreleme yöntemleridir. 
         * Bu algoritma türünde veriyi şifreleyen taraf ile veriyi çözecek olan taraf birbirinden farklı anahtarlar kullanırlar.
         */
        public string Asimetrik_RSASifrele(string data, out RSAParameters prm)
        {
            string result = "";
            if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException("Şifrelenecek veri yok");
            else
            {
                byte[] aryDizi = ByteDonustur(data);
                RSACryptoServiceProvider dec = new RSACryptoServiceProvider();
                prm = dec.ExportParameters(true);
                byte[] aryDonus = dec.Encrypt(aryDizi, false);
                result = Convert.ToBase64String(aryDonus);
            }
            return result;
        }

        public string Asimetrik_RSACoz(string data, RSAParameters prm)
        {
            string result = "";
            if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException("Şifrelenecek veri yok");
            else
            {
                RSACryptoServiceProvider dec = new RSACryptoServiceProvider();
                byte[] aryDizi = Convert.FromBase64String(data);
                UnicodeEncoding UE = new UnicodeEncoding();
                dec.ImportParameters(prm);
                byte[] aryDonus = dec.Decrypt(aryDizi, false);
                result = UE.GetString(aryDonus);
            }
            return result;
        }
        #endregion
    }
}
