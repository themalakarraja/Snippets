using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DropboxFileUpload
{
    public class FtpHelper
    {
        private const string USERNAME = "";
        private const string PASSWORD = "";

        private FtpWebRequest CreateRequest(string url, string method)
        {
            var request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.Credentials = new NetworkCredential(USERNAME, PASSWORD);
            return request;
        }

        public void Download(string downloadFtpPath, string saveDir)
        {
            WebClient request = new WebClient();
            request.Credentials = new NetworkCredential(USERNAME, PASSWORD);
            byte[] fileData = request.DownloadData(downloadFtpPath);
            saveDir = Path.Combine(saveDir, Path.GetFileName(downloadFtpPath));
            File.WriteAllBytes(saveDir, fileData);
        }

        private long FileSize(string ftpPath)
        {
            var request = CreateRequest(ftpPath, WebRequestMethods.Ftp.GetFileSize);
            try 
            {
                return ((FtpWebResponse)request.GetResponse()).ContentLength;
            }
            catch (Exception) 
            {
                return default(long);
            }
        }

        public List<string> FileList(string url)
        {
            List<string> list = new List<string>();
            try
            {
                FtpWebRequest request = CreateRequest(url, WebRequestMethods.Ftp.ListDirectory);
                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream, true))
                        {
                            while (!reader.EndOfStream)
                            {
                                string fileName = reader.ReadLine();
                                long fileSize = FileSize(url + fileName);
                                if (fileSize > 0)
                                {
                                    list.Add(fileName);
                                }
                            }
                        }
                    }
                }
            }
            catch (WebException e)
            {
                string status = ((FtpWebResponse)e.Response).StatusDescription;
                list = null;
            }
            return list;
        }
    }
}
