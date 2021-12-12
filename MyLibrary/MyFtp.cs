using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MyLibrary
{
    public sealed class MyFtp
    {
        private static readonly Lazy<MyFtp> lazy = new Lazy<MyFtp>(() => new MyFtp());
        public static MyFtp Instance { get { return lazy.Value; } }
        private MyFtp()
        {

        }
        public class ftpParametreler
        {
            public string ftpDirectory { get; set; }
            public string ftpFileName { get; set; }
            public string ftpUrlPath { get; set; }
            public string LocalDirectory { get; set; }
            public string LocalFileName { get; set; }
            public int port { get; set; }
            public string UserName { get; set; }
            public string UserPassword { get; set; }
        }


        public bool DownloadFile(string url, string destination, out Exception _hata)
        {
            _hata = null;
            bool flag = false;
            HttpWebRequest request = null;
            WebResponse response = null;
            Stream responseStream = null;
            FileStream stream2 = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = 100000;
                response = request.GetResponse();
                responseStream = response.GetResponseStream();
                stream2 = File.Open(destination, FileMode.Create, FileAccess.Write, FileShare.None);
                int count = 10240;
                byte[] buffer = new byte[count];
                int num2 = 0;
                int num3 = 0;
                //response.ContentLength
                while ((num2 = responseStream.Read(buffer, 0, count)) > 0)
                {
                    num3 += num2;
                    stream2.Write(buffer, 0, num2);
                }
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                _hata = ex;
            }
            finally
            {
                if (responseStream != null)
                    responseStream.Close();
                if (response != null)
                    response.Close();
                if (stream2 != null)
                    stream2.Close();
            }
            if (!flag && File.Exists(destination))
            {
                File.Delete(destination);
            }
            return flag;
        }

        public List<string> ShowFileList(ftpParametreler param, out Exception _hata)
        {
            _hata = null;
            string str;
            List<string> list = new List<string>();
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(param.ftpUrlPath + "/" + param.ftpDirectory);
                request.Credentials = new NetworkCredential(param.UserName, param.UserPassword);
                request.Method = "NLST";
                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
                while ((str = reader.ReadLine()) != null)
                    list.Add(str);
                reader.Close();
            }
            catch (Exception ex)
            {
                _hata = ex;
            }
            return list;
        }

        public void ftpDownloadFile(ftpParametreler param, out Exception _hata)
        {
            _hata = null;
            if (!File.Exists(param.LocalDirectory + @"\" + param.ftpFileName))
            {
                try
                {
                    string[] textArray1 = new string[] { param.ftpUrlPath, "/", param.ftpDirectory, "/", param.ftpFileName };
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(string.Concat(textArray1));
                    request.Credentials = new NetworkCredential(param.UserName, param.UserPassword);
                    request.Method = "RETR";
                    Stream responseStream = ((FtpWebResponse)request.GetResponse()).GetResponseStream();
                    FileStream stream2 = new FileStream(param.LocalDirectory + @"\" + param.ftpFileName, FileMode.Create);
                    int count = 0x800;
                    byte[] buffer = new byte[count];
                    for (int i = responseStream.Read(buffer, 0, count); i > 0; i = responseStream.Read(buffer, 0, count))
                        stream2.Write(buffer, 0, i);
                    responseStream.Close();
                    stream2.Close();
                    request = null;
                }
                catch (Exception exception)
                {
                    _hata= exception;
                }
            }
        }

        public bool ftpUploadFile(ftpParametreler param, out Exception _hata)
        {
            _hata = null;
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(param.ftpUrlPath));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(param.UserName, param.UserPassword);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                _hata = ex;
            }
            try
            {
                FileInfo info = new FileInfo(Path.Combine(param.LocalDirectory , param.LocalFileName));
                FtpWebRequest request2 = (FtpWebRequest)WebRequest.Create(Path.Combine(param.ftpUrlPath, param.ftpDirectory, info.Name).Replace("\\", "/"));
                request2.Method = WebRequestMethods.Ftp.UploadFile;
                request2.Credentials = new NetworkCredential(param.UserName, param.UserPassword);
                request2.Proxy = null;
                StreamReader reader = new StreamReader(Path.Combine(param.LocalDirectory, info.Name));
                byte[] bytes = Encoding.UTF8.GetBytes(reader.ReadToEnd());
                reader.Close();
                request2.ContentLength = bytes.Length;
                Stream requestStream = request2.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                return true;
            }
            catch (Exception ex)
            {
                _hata = ex;
                return false;
            }
        }

        public void ftpDeleteFile(ftpParametreler param, out Exception _hata)
        {
            _hata = null;
            try
            {
                string[] textArray1 = new string[] { param.ftpUrlPath, "/", param.ftpDirectory, "/", param.ftpFileName };
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(string.Concat(textArray1));
                request.Credentials = new NetworkCredential(param.UserName, param.UserPassword);
                request.Method = "DELE";
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                _hata = ex;
            }
        }
    }
}
