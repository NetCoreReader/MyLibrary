using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MyLibrary
{
    public class MySqlCmd
    {
        private static Lazy<MySqlCmd> lazy = new Lazy<MySqlCmd>(() => new MySqlCmd());
        public static MySqlCmd Instance { get { return lazy.Value; } }
        public MySqlCmd()
        {

        }

        public void sqlBackUpFile(string connectionString, string klasorYolu, bool otomatikBaslat)
        {
            try
            {
                if (MyConnect.Instance.ConnectionControl(connectionString, out MyConnect.Instance.exp))
                    using (StreamWriter sw = new StreamWriter(klasorYolu + "\\SqlBackUp.bat"))
                    {
                        var result = MyConnect.Instance.ConnectionStringB(connectionString);
                        if (String.IsNullOrEmpty(result.UserID) || String.IsNullOrWhiteSpace(result.UserID))
                            sw.WriteLine($"Sqlcmd -S {result.DataSource} -Q \"BACKUP DATABASE [{result.InitialCatalog}] TO DISK = '{klasorYolu}\\{DateTime.Now.Year}{DateTime.Now.Month.ToString().PadLeft(2, '0')}{DateTime.Now.Day.ToString().PadLeft(2, '0')}_{DateTime.Now.Hour.ToString().PadLeft(2, '0')}{DateTime.Now.Minute.ToString().PadLeft(2, '0')}_{result.InitialCatalog}.BAK'\"");
                        else
                            sw.WriteLine($"Sqlcmd -S {result.DataSource} -U {result.UserID} -P {result.Password} -Q \"BACKUP DATABASE [{result.InitialCatalog}] TO DISK = '{klasorYolu}\\{DateTime.Now.Year}{DateTime.Now.Month.ToString().PadLeft(2, '0')}{DateTime.Now.Day.ToString().PadLeft(2, '0')}_{DateTime.Now.Hour.ToString().PadLeft(2, '0')}{DateTime.Now.Minute.ToString().PadLeft(2, '0')}_{result.InitialCatalog}.BAK'\"");
                    }
                else
                    throw new Exception("Bağlantı Cümlesi Hatalı: " + MyConnect.Instance.exp.Message);
                if (otomatikBaslat)
                    System.Diagnostics.Process.Start(klasorYolu + "\\SqlBackUp.bat");
            }
            catch (Exception ex)
            {
                throw new Exception("Process Hatası: " + ex.Message);
            }
        }

        public void sqlChangePassword(string dataSource, string newPass, bool otomatikBaslat)
        {
            try
            {
                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();

                cmd.StandardInput.WriteLine($"Sqlcmd -S {dataSource} -E");
                cmd.StandardInput.WriteLine($"sp_password NULL, '{newPass}', 'sa'");
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                Console.WriteLine(cmd.StandardOutput.ReadToEnd());

         //sw.WriteLine($"Sqlcmd -S {dataSource} -E");
         //           sw.WriteLine($"sp_password NULL, '{newPass}', 'sa'");
         //           sw.WriteLine($"GO");
         //           sw.WriteLine($"exit");
         
            }
            catch (Exception ex)
            {
                throw new Exception("Process Hatası: " + ex.Message);
            }
        }
    }
}
