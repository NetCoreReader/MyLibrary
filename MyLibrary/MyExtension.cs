using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MyLibrary
{
    public static class MyExtension
    {
        /// <summary>
        /// Bir string içerisindeki tüm karakterlerin Ascii değerlerini ele alıp byte dizisi şeklinde geriye döndürür. Eğer string null veya empty ise exception döndürür.
        /// </summary>
        /// <param name="s">Byte değerleri döndürülecek string parametre</param>
        /// <returns>Ascii değerleri</returns>
        public static byte[] _GetAscii(this string s)
        {
            if (String.IsNullOrEmpty(s))
                throw new Exception("String veri olmalıdır.");
            byte[] result = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                result[i] = (byte)s[i];
            }
            return result;
        }
        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase: true);
        }
        public static bool? _ToBoolean(this object gelen)
        {
            bool? nullable;
            try
            {
                if (gelen == null) throw new Exception();
                nullable = new bool?(Convert.ToBoolean(gelen));
            }
            catch (Exception)
            {
                try
                {
                    if (gelen == DBNull.Value) throw new Exception();
                    nullable = new bool?(Convert.ToBoolean(gelen));
                }
                catch (Exception)
                {
                    nullable = null;
                }
            }
            return nullable;
        }

        public static bool _ToBooleanR(this object gelen)
        {
            bool flag3;
            try
            {
                if (gelen == null) throw new Exception();
                flag3 = Convert.ToBoolean(gelen);
            }
            catch (Exception)
            {
                try
                {
                    if (gelen == DBNull.Value) throw new Exception();
                    flag3 = Convert.ToBoolean(gelen);
                }
                catch (Exception)
                {
                    flag3 = false;
                }
            }
            return flag3;
        }

        public static DateTime? _ToDateTime(this object gelen)
        {
            DateTime? nullable;
            try
            {
                if (gelen == null) throw new Exception();
                nullable = new DateTime?(Convert.ToDateTime(gelen));
            }
            catch (Exception)
            {
                try
                {
                    if (gelen == DBNull.Value) throw new Exception();
                    nullable = new DateTime?(Convert.ToDateTime(gelen));
                }
                catch (Exception)
                {
                    nullable = null;
                }
            }
            return nullable;
        }

        public static DateTime _ToDateTimeR(this object gelen)
        {
            DateTime minValue;
            try
            {
                if (gelen == null) throw new Exception();
                minValue = Convert.ToDateTime(gelen);
            }
            catch (Exception)
            {
                try
                {
                    if (gelen == DBNull.Value) throw new Exception();
                    minValue = Convert.ToDateTime(gelen);
                }
                catch (Exception)
                {
                    minValue = DateTime.MinValue;
                }
            }
            return minValue;
        }

        public static DateTime _ToDateTimeOnly(this DateTime gelen)
        {
            DateTime minValue;
            try
            {
                if (gelen == null) throw new Exception();
                minValue = new DateTime(gelen.Year, gelen.Month, gelen.Day);
            }
            catch (Exception)
            {
                try
                {
                    minValue = new DateTime(gelen.Year, gelen.Month, gelen.Day);
                }
                catch (Exception)
                {
                    minValue = DateTime.MinValue;
                }
            }
            return minValue;
        }

        public static DateTime _FromExcelIntToDatetime(this int a)
        {
            if (a > 59) a -= 1;
            return new DateTime(1899, 12, 31).AddDays(a);
        }

        public static decimal? _ToDecimal(this object gelen)
        {
            decimal? nullable;
            try
            {
                if (gelen == null) throw new Exception();
                nullable = new decimal?(Convert.ToDecimal(gelen));
            }
            catch (Exception)
            {
                try
                {
                    if (gelen == DBNull.Value) throw new Exception();
                    nullable = new decimal?(Convert.ToDecimal(gelen));
                }
                catch (Exception)
                {
                    nullable = null;
                }
            }
            return nullable;
        }

        public static decimal _ToDecimalR(this object gelen)
        {
            decimal zero;
            try
            {
                if (gelen == null) throw new Exception();
                zero = Convert.ToDecimal(gelen);
            }
            catch (Exception)
            {
                try
                {
                    if (gelen == DBNull.Value) throw new Exception();
                    zero = Convert.ToDecimal(gelen);
                }
                catch (Exception)
                {
                    zero = decimal.Zero;
                }
            }
            return zero;
        }

        public static double? _ToDouble(this object gelen)
        {
            double? nullable;
            try
            {
                if (gelen == null) throw new Exception();
                nullable = new double?(Convert.ToDouble(gelen));
            }
            catch (Exception)
            {
                try
                {
                    if (gelen == DBNull.Value) throw new Exception();
                    nullable = new double?(Convert.ToDouble(gelen));
                }
                catch (Exception)
                {
                    nullable = null;
                }
            }
            return nullable;
        }

        public static double _ToDoubleR(this object gelen)
        {
            double num2;
            try
            {
                if (gelen == null) throw new Exception();
                num2 = Convert.ToDouble(gelen);
            }
            catch (Exception)
            {
                try
                {
                    if (gelen == DBNull.Value) throw new Exception();
                    num2 = Convert.ToDouble(gelen);
                }
                catch (Exception)
                {
                    num2 = 0.0;
                }
            }
            return num2;
        }

        public static double _StringToDouble(this string gelen)
        {
            if (String.IsNullOrEmpty(gelen) || String.IsNullOrWhiteSpace(gelen))
                return 0;
            else
                return gelen.Replace(MyTools.Instance.OndalikYanSperator, MyTools.Instance.OndalikSperator)._ToDoubleR();
        }
        /// <summary>
        /// Verilen ifadeyi float cinsine dönüştürür.
        /// </summary>
        /// <param name="gelen">Gelen İfadesi Stringe Dönüştürülecektir.</param>
        /// <returns>float</returns>
        public static float? _ToFloat(this object gelen)
        {
            float? nullable;
            try
            {
                if (gelen == null) throw new Exception();
                nullable = new float?(float.Parse(gelen.ToString()));
            }
            catch (Exception)
            {
                try
                {
                    if (string.IsNullOrEmpty(gelen.ToString()) || string.IsNullOrWhiteSpace(gelen.ToString())) throw new Exception();
                    nullable = new float?(float.Parse(gelen.ToString()));
                }
                catch (Exception)
                {
                    nullable = null;
                }
            }
            return nullable;
        }
        /// <summary>
        /// Verilen ifadeyi float cinsine dönüştürür.
        /// </summary>
        /// <param name="gelen">Gelen İfadesi Stringe Dönüştürülecektir.</param>
        /// <returns>float</returns>
        public static float _ToFloatR(this object gelen)
        {
            float num2;
            try
            {
                if (gelen == null) throw new Exception();
                num2 = float.Parse(gelen.ToString());
            }
            catch (Exception)
            {
                try
                {
                    if (string.IsNullOrEmpty(gelen.ToString()) || string.IsNullOrWhiteSpace(gelen.ToString())) throw new Exception();
                    num2 = float.Parse(gelen.ToString());
                }
                catch (Exception)
                {
                    num2 = 0f;
                }
            }
            return num2;
        }

        public static int? _ToInteger(this object gelen)
        {
            int? nullable;
            try
            {
                if (gelen == null) throw new Exception();
                nullable = new int?(Convert.ToInt32(gelen));
            }
            catch (Exception)
            {
                try
                {
                    if (gelen == DBNull.Value) throw new Exception();
                    nullable = new int?(Convert.ToInt32(gelen));
                }
                catch (Exception)
                {
                    nullable = null;
                }
            }
            return nullable;
        }

        public static int _ToIntegerR(this object gelen)
        {
            int num2;
            try
            {
                if (gelen == null) throw new Exception();
                num2 = Convert.ToInt32(gelen);
            }
            catch (Exception)
            {
                try
                {
                    if (gelen == DBNull.Value) throw new Exception();
                    num2 = Convert.ToInt32(gelen);
                }
                catch (Exception)
                {
                    num2 = 0;
                }
            }
            return num2;
        }

        public static string _ToString(this object gelen)
        {
            string str;
            try
            {
                if (gelen == null) throw new Exception();
                str = gelen.ToString();
            }
            catch (Exception)
            {
                try
                {
                    if (gelen == DBNull.Value) throw new Exception();
                    str = gelen.ToString();
                }
                catch (Exception)
                {
                    str = "";
                }
            }
            return str;
        }

        public static bool _ToBosmu(this string gelen)
        {
            try
            {
                return String.IsNullOrEmpty(gelen) || String.IsNullOrWhiteSpace(gelen);
            }
            catch (Exception)
            {
                return true;
            }
        }

        public static string _HTMLKodTemizleyici(this string gelenMetin)
        {
            Regex regex = new Regex("<.*?>", RegexOptions.Compiled);
            return regex.Replace(gelenMetin, string.Empty);
        }

        public static string _SqlBugTemizle(this string str)
        {
            str = str.Replace("'", "");
            str = str.Replace(";", "");
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            str = str.Replace("[", "");
            str = str.Replace("]", "");
            str = str.Replace("{", "");
            str = str.Replace("}", "");
            str = str.Replace("=", "");
            str = str.Replace("`", "");
            str = str.Replace("&", "");
            str = str.Replace("%", "");
            str = str.Replace("!", "");
            str = str.Replace("#", "");
            str = str.Replace("<", "");
            str = str.Replace(">", "");
            str = str.Replace("*", "");
            str = str.Replace("-", "");
            str = str.Replace("--", "");
            str = str.Replace(" and ", "");
            str = str.Replace("delay", "");
            str = str.Replace("DELAY", "");
            str = str.Replace("WAITFOR", "");
            str = str.Replace("waitfor", "");
            return str;
        }

        public static string _TurkceKarakterleriTemizle(this string str)
        {
            return str.Replace('ı', 'i').Replace('ö', 'o').Replace('ü', 'u').Replace('ş', 's').Replace('ğ', 'g').Replace('ç', 'c').Replace('İ', 'I').Replace('Ö', 'O').Replace('Ü', 'U').Replace('Ş', 'S').Replace('Ğ', 'G').Replace('Ç', 'C');
        }

        private static readonly HashSet<char> _NonWordCharacters = new HashSet<char> { ',', '.', ':', ';' };

        public static string Crop(this string value, int length = 0)
        {
            if (length == 0 || length >= value.Length)
            {
                return value;
            }
            int end = length;

            for (var i = end; i > 0; i--)
            {
                if (string.IsNullOrWhiteSpace(value[i].ToString()))
                {
                    break;
                }

                if (_NonWordCharacters.Contains(value[i]) && (value.Length == i + 1 || value[i + 1] == ' '))
                {
                    break;
                }
                end--;
            }

            if (end == 0)
            {
                end = length;
            }

            return value.Substring(0, end);
        }

        public enum KontrolTipleri
        {
            TC_Kimlik_No,
            VergiKimlik_No,
            IBAN_No,
            Email,
            KrediKarti,
            IP_Adress
        }

        public static bool KontrolSistemi(this string gelenVeri, KontrolTipleri kontrolu)
        {
            #region TC Kimlik
            if (kontrolu == KontrolTipleri.TC_Kimlik_No)
            {
                string str = gelenVeri;
                try
                {
                    int num = 0;
                    int num2 = 0;
                    foreach (char ch in str)
                    {
                        if (num < 10)
                            num2 += Convert.ToInt32(char.ToString(ch));
                        num++;
                    }
                    char ch2 = str[10];
                    return ((num2 % 10) == Convert.ToInt32(ch2.ToString()));
                }
                catch
                {
                    return false;
                }
            }
            #endregion
            #region Vergi Kimlik No
            else if (kontrolu == KontrolTipleri.VergiKimlik_No)
            {
                string str3 = gelenVeri;
                try
                {
                    int num4 = (Convert.ToInt32(gelenVeri.Substring(0, 1)) + 9) % 10;
                    int num5 = (Convert.ToInt32(gelenVeri.Substring(1, 1)) + 8) % 10;
                    int num6 = (Convert.ToInt32(gelenVeri.Substring(2, 1)) + 7) % 10;
                    int num7 = (Convert.ToInt32(gelenVeri.Substring(3, 1)) + 6) % 10;
                    int num8 = (Convert.ToInt32(gelenVeri.Substring(4, 1)) + 5) % 10;
                    int num9 = (Convert.ToInt32(gelenVeri.Substring(5, 1)) + 4) % 10;
                    int num10 = (Convert.ToInt32(gelenVeri.Substring(6, 1)) + 3) % 10;
                    int num11 = (Convert.ToInt32(gelenVeri.Substring(7, 1)) + 2) % 10;
                    int num12 = (Convert.ToInt32(gelenVeri.Substring(8, 1)) + 1) % 10;
                    int num13 = Convert.ToInt32(gelenVeri.Substring(9, 1)) % 10;
                    int num14 = (num4 * 0x200) % 9;
                    int num15 = (num5 * 0x100) % 9;
                    int num16 = (num6 * 0x80) % 9;
                    int num17 = (num7 * 0x40) % 9;
                    int num18 = (num8 * 0x20) % 9;
                    int num19 = (num9 * 0x10) % 9;
                    int num20 = (num10 * 8) % 9;
                    int num21 = (num11 * 4) % 9;
                    int num22 = (num12 * 2) % 9;
                    if ((num4 != 0) && (num14 == 0)) num14 = 9;
                    if ((num5 != 0) && (num15 == 0)) num15 = 9;
                    if ((num6 != 0) && (num16 == 0)) num16 = 9;
                    if ((num7 != 0) && (num17 == 0)) num17 = 9;
                    if ((num8 != 0) && (num18 == 0)) num18 = 9;
                    if ((num9 != 0) && (num19 == 0)) num19 = 9;
                    if ((num10 != 0) && (num20 == 0)) num20 = 9;
                    if ((num11 != 0) && (num21 == 0)) num21 = 9;
                    if ((num12 != 0) && (num22 == 0)) num22 = 9;
                    int num23 = (((((((num14 + num15) + num16) + num17) + num18) + num19) + num20) + num21) + num22;
                    if ((num23 % 10) == 0) num23 = 0;
                    else num23 = 10 - (num23 % 10);
                    return (num23 == num13);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            #endregion
            #region IBAN No
            else if (kontrolu == KontrolTipleri.IBAN_No)
            {
                string str4 = gelenVeri.ToUpper();
                if (!string.IsNullOrEmpty(str4) && Regex.IsMatch(str4, "^[A-Z0-9]"))
                {
                    str4 = str4.Replace(" ", string.Empty);
                    string str5 = str4.Substring(4, str4.Length - 4) + str4.Substring(0, 4);
                    int num24 = 0x37;
                    StringBuilder builder = new StringBuilder();
                    foreach (char ch3 in str5)
                    {
                        int num27;
                        if (char.IsLetter(ch3))
                        {
                            num27 = ch3 - num24;
                        }
                        else
                        {
                            num27 = int.Parse(ch3.ToString());
                        }
                        builder.Append(num27);
                    }
                    string str6 = builder.ToString();
                    int num25 = int.Parse(str6.Substring(0, 1));
                    for (int i = 1; i < str6.Length; i++)
                    {
                        int num29 = int.Parse(str6.Substring(i, 1));
                        num25 *= 10;
                        num25 += num29;
                        num25 = num25 % 0x61;
                    }
                    return (num25 == 1);
                }
                return false;
            }
            #endregion
            #region Mail Hesabı
            else if (kontrolu == KontrolTipleri.Email)
                return Regex.IsMatch(gelenVeri, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            #endregion
            #region Kredi Kartı
            else if (kontrolu == KontrolTipleri.KrediKarti)
            {
                int toplam = 0;
                for (int i = 0; i < 16; i++)
                {
                    int sayi = Convert.ToInt32(gelenVeri[i].ToString());

                    if (i % 2 == 0)
                    {
                        sayi = sayi * 2;
                        if (sayi.ToString().Length == 2)
                            sayi = Convert.ToInt32(sayi.ToString().Substring(0, 1)) + Convert.ToInt32(sayi.ToString().Substring(1, 1));
                    }

                    toplam += sayi;
                }

                if (toplam % 10 == 0)
                    return true;
                else
                    return false;
            }
            #endregion
            #region IP Adres
            else if (kontrolu == KontrolTipleri.IP_Adress)
                return Regex.IsMatch(gelenVeri, @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$");
            #endregion
            else
                return false;
        }
    }
}
